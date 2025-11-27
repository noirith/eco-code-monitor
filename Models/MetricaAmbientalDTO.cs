namespace ErroLoggerAPI.Models;

public class MetricaAmbientalDTO
{
    public string? Id { get; set; }
    public string AplicacaoNome { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string Ambiente { get; set; } = string.Empty;
    public double CpuUsagePercent { get; set; }
    public long MemoriaUsadaMB { get; set; }
    public long DuracaoMs { get; set; }
    public int NumeroRequisicoes { get; set; }
    public string TipoOperacao { get; set; } = string.Empty;
    public double EnergiaConsumidaWh { get; set; }
    public double EmissaoCO2Gramas { get; set; }
    public string CarbonScore { get; set; } = string.Empty;
    public string DataHora { get; set; } = string.Empty;
}