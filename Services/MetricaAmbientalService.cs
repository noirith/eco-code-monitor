using ErroLoggerAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ErroLoggerAPI.Services;

public class MetricaAmbientalService
{
    private readonly IMongoCollection<MetricaAmbiental> _metricas;
    private readonly CalculadoraAmbientalService _calculadora;
    
    public MetricaAmbientalService(
        IOptions<MongoDbSettings> settings,
        CalculadoraAmbientalService calculadora)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _metricas = database.GetCollection<MetricaAmbiental>("MetricasAmbientais");
        _calculadora = calculadora;
    }
    
    /// <summary>
    /// Salva uma nova métrica com cálculos ambientais
    /// </summary>
    public async Task<MetricaAmbiental> SalvarMetricaAsync(MetricaAmbiental metrica)
    {
        // Define data/hora atual
        metrica.DataHora = DateTime.UtcNow;
        
        // Calcula impacto ambiental
        metrica = _calculadora.CalcularImpactoAmbiental(metrica);
        
        // Salva no MongoDB
        await _metricas.InsertOneAsync(metrica);
        
        return metrica;
    }
    
    /// <summary>
    /// Obtém métricas paginadas com filtros
    /// </summary>
    public async Task<List<MetricaAmbientalDTO>> ObterMetricasPaginadasAsync(
        int pagina,
        int tamanhoPagina,
        string? aplicacao = null,
        DateTime? dataInicio = null,
        DateTime? dataFim = null,
        string? ambiente = null)
    {
        var filtro = Builders<MetricaAmbiental>.Filter.Empty;
        
        // Filtro por aplicação
        if (!string.IsNullOrEmpty(aplicacao))
        {
            filtro &= Builders<MetricaAmbiental>.Filter.Eq(m => m.AplicacaoNome, aplicacao);
        }
        
        // Filtro por data início
        if (dataInicio.HasValue)
        {
            filtro &= Builders<MetricaAmbiental>.Filter.Gte(m => m.DataHora, dataInicio.Value);
        }
        
        // Filtro por data fim
        if (dataFim.HasValue)
        {
            filtro &= Builders<MetricaAmbiental>.Filter.Lte(m => m.DataHora, dataFim.Value);
        }
        
        // Filtro por ambiente
        if (!string.IsNullOrEmpty(ambiente))
        {
            filtro &= Builders<MetricaAmbiental>.Filter.Eq(m => m.Ambiente, ambiente);
        }
        
        var metricas = await _metricas
            .Find(filtro)
            .SortByDescending(m => m.DataHora)
            .Skip((pagina - 1) * tamanhoPagina)
            .Limit(tamanhoPagina)
            .ToListAsync();
        
        return metricas.Select(m => new MetricaAmbientalDTO
        {
            Id = m.Id,
            AplicacaoNome = m.AplicacaoNome,
            Endpoint = m.Endpoint,
            Ambiente = m.Ambiente,
            CpuUsagePercent = Math.Round(m.CpuUsagePercent, 2),
            MemoriaUsadaMB = m.MemoriaUsadaMB,
            DuracaoMs = m.DuracaoMs,
            NumeroRequisicoes = m.NumeroRequisicoes,
            TipoOperacao = m.TipoOperacao,
            EnergiaConsumidaWh = Math.Round(m.EnergiaConsumidaWh, 4),
            EmissaoCO2Gramas = Math.Round(m.EmissaoCO2Gramas, 4),
            CarbonScore = m.CarbonScore,
            DataHora = m.DataHora.ToString("dd/MM/yyyy HH:mm:ss")
        }).ToList();
    }
   
  
    /// <summary>
    /// Obtém lista de aplicações únicas
    /// </summary>
    public async Task<List<string>> ObterAplicacoesAsync()
    {
        var aplicacoes = await _metricas
            .Distinct<string>("aplicacaoNome", Builders<MetricaAmbiental>.Filter.Empty)
            .ToListAsync();
        
        return aplicacoes;
    }
    
    /// <summary>
    /// APENAS PARA DESENVOLVIMENTO - Deleta todas as métricas
    /// </summary>
    public async Task<long> LimparTodasMetricasAsync()
    {
        var resultado = await _metricas.DeleteManyAsync(Builders<MetricaAmbiental>.Filter.Empty);
        return resultado.DeletedCount;
    }
}