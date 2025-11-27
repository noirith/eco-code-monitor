using Microsoft.AspNetCore.Mvc;
using ErroLoggerAPI.Models;
using ErroLoggerAPI.Services;

namespace ErroLoggerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetricasAmbientaisController : ControllerBase
{
    private readonly MetricaAmbientalService _metricaService;
    
    public MetricasAmbientaisController(MetricaAmbientalService metricaService)
    {
        _metricaService = metricaService;
    }
    
    /// <summary>
    /// Registra uma nova métrica ambiental
    /// </summary>
    /// <param name="metrica">Dados da métrica a ser registrada</param>
    /// <returns>Métrica registrada com cálculos ambientais</returns>
    [HttpPost]
    [ProducesResponseType(typeof(MetricaAmbiental), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegistrarMetrica([FromBody] MetricaAmbiental metrica)
    {
        if (string.IsNullOrEmpty(metrica.AplicacaoNome) || string.IsNullOrEmpty(metrica.Endpoint))
        {
            return BadRequest("AplicacaoNome e Endpoint são obrigatórios");
        }
        
        var metricaSalva = await _metricaService.SalvarMetricaAsync(metrica);
        
        return CreatedAtAction(
            nameof(ObterMetricas),
            new { id = metricaSalva.Id },
            metricaSalva);
    }
    
    /// <summary>
    /// Obtém métricas com paginação e filtros
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<MetricaAmbientalDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterMetricas(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 20,
        [FromQuery] string? aplicacao = null,
        [FromQuery] DateTime? dataInicio = null,
        [FromQuery] DateTime? dataFim = null,
        [FromQuery] string? ambiente = null)
    {
        var metricas = await _metricaService.ObterMetricasPaginadasAsync(
            pagina,
            tamanhoPagina,
            aplicacao,
            dataInicio,
            dataFim,
            ambiente);
        
        return Ok(metricas);
    }
    
    /// <summary>
    /// Lista todas as aplicações cadastradas
    /// </summary>
    [HttpGet("aplicacoes")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarAplicacoes()
    {
        var aplicacoes = await _metricaService.ObterAplicacoesAsync();
        return Ok(aplicacoes);
    }
    
    /// <summary>
    /// APENAS PARA DESENVOLVIMENTO - Limpa todas as métricas
    /// </summary>
    [HttpDelete("limpar-tudo")]
    public async Task<IActionResult> LimparTudo()
    {
        try
        {
            var resultado = await _metricaService.LimparTodasMetricasAsync();
            return Ok(new { 
                mensagem = "Banco limpo com sucesso!", 
                registrosDeletados = resultado 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { erro = ex.Message });
        }
    }
}