using ErroLoggerAPI.Models;

namespace ErroLoggerAPI.Services;

public class CalculadoraAmbientalService
{
    // Fator ajustado para gerar a escala correta
    private const double FATOR_BASE = 0.001; // Reduzido 20x (de 0.02 para 0.001)
    
    public MetricaAmbiental CalcularImpactoAmbiental(MetricaAmbiental metrica)
    {
        // Calcular pontos de impacto
        double pontos = (metrica.CpuUsagePercent / 10.0) * 
                       (metrica.DuracaoMs / 100.0) * 
                       (metrica.MemoriaUsadaMB / 1000.0);
        
        // CO2 por requisição
        double co2PorRequisicao = pontos * FATOR_BASE;
        
        // Score ANTES de multiplicar
        metrica.CarbonScore = CalcularCarbonScore(co2PorRequisicao);
        
        // Total
        metrica.EmissaoCO2Gramas = co2PorRequisicao * metrica.NumeroRequisicoes;
        metrica.EnergiaConsumidaWh = metrica.EmissaoCO2Gramas / 0.0385;
        
        return metrica;
    }
    
    private string CalcularCarbonScore(double co2PorReq)
    {
        //  thresholds em GRAMAS
        if (co2PorReq < 0.001) return "A";    // < 1mg
        if (co2PorReq < 0.01) return "B";     // 1-10mg
        if (co2PorReq < 0.1) return "C";      // 10-100mg
        if (co2PorReq < 1.0) return "D";      // 100mg-1g
        return "E";                            // > 1g
    }
}