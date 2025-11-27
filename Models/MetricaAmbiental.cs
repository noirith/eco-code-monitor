using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ErroLoggerAPI.Models;

public class MetricaAmbiental
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    // Identificação da aplicação
    [BsonElement("aplicacaoNome")]
    public string AplicacaoNome { get; set; } = string.Empty;
    
    [BsonElement("endpoint")]
    public string Endpoint { get; set; } = string.Empty;
    
    [BsonElement("ambiente")]
    public string Ambiente { get; set; } = "production"; // dev, staging, production
    
    [BsonElement("versao")]
    public string? Versao { get; set; }
    
    // Métricas técnicas coletadas
    [BsonElement("cpuUsagePercent")]
    public double CpuUsagePercent { get; set; }
    
    [BsonElement("memoriaUsadaMB")]
    public long MemoriaUsadaMB { get; set; }
    
    [BsonElement("duracaoMs")]
    public long DuracaoMs { get; set; }
    
    [BsonElement("numeroRequisicoes")]
    public int NumeroRequisicoes { get; set; } = 1;
    
    [BsonElement("tipoOperacao")]
    public string TipoOperacao { get; set; } = "Processing"; // Query, Processing, I/O, Network
    
    // Cálculos ambientais (serão preenchidos pela API)
    [BsonElement("energiaConsumidaWh")]
    public double EnergiaConsumidaWh { get; set; }
    
    [BsonElement("emissaoCO2Gramas")]
    public double EmissaoCO2Gramas { get; set; }
    
    [BsonElement("carbonScore")]
    public string CarbonScore { get; set; } = "C"; // A, B, C, D, E
    
    // Metadados
    [BsonElement("dataHora")]
    public DateTime DataHora { get; set; }
    
    [BsonElement("dadosAdicionais")]
    public Dictionary<string, string>? DadosAdicionais { get; set; }
}