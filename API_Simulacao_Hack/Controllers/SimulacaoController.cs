using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.Enum;
using API_Simulacao_Hack.Interfaces.Services;
using API_Simulacao_Hack.Util.Base;
using API_Simulacao_Hack.Wrappers.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API_Simulacao_Hack.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class SimulacaoController : ControllerBase
    {
        private readonly ISimulacaoService _simulacaoService;

        public SimulacaoController(ISimulacaoService simulacaoService)
        {
            _simulacaoService = simulacaoService;
        }

        [HttpPost]
        [Route("simula")]
        [SwaggerOperation(
            Summary = "Simulação de Crédito.",
            Description = "Simular oferta de crédito."
        )]
        [SwaggerResponse(200, "Retorna simulação realizada")]
        [SwaggerResponse(400, "Erro na requisição.")]
        [SwaggerResponse(404, "Produto não encontrado.")]
        [SwaggerResponse(500, "Erro interno do servidor.")]
        public async Task<IActionResult> Simulacao([FromBody][SwaggerParameter("Solicitação de simulação")] SolicitacaoSimulacaoDTO simulacaoDTO, CancellationToken cancellationToken)
        {
            try
            {
                var simulacao = await _simulacaoService.RealizaSimulacao(simulacaoDTO);

                if(simulacao.StatusCode != StatusCodes.Status200OK)
                {
                    return StatusCode(simulacao.StatusCode, simulacao);
                }

                return StatusCode(simulacao.StatusCode, simulacao.Data);
            }
            catch (CustomException ex)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("listaSimulacoes")]
        [SwaggerOperation(
            Summary = "Lista Simulações",
            Description = "Lista as simulações"
        )]
        [SwaggerResponse(200, "Retorna lista de simulações paginada")]
        [SwaggerResponse(400, "Erro na requisição.")]
        [SwaggerResponse(500, "Erro interno do servidor.")]
        public async Task<IActionResult> ListaSimulacoes(int pagina, int qtdRegistrosPagina, TipoSimulacaoEnum? tipoSimulacao, CancellationToken cancellationToken)
        {
            try
            {
                var lsSimulacao = await _simulacaoService.ListaSimulacoes(pagina, qtdRegistrosPagina, tipoSimulacao.ToString());

                if (lsSimulacao.StatusCode != StatusCodes.Status200OK)
                {
                    return StatusCode(lsSimulacao.StatusCode, lsSimulacao);
                }

                return StatusCode(lsSimulacao.StatusCode, lsSimulacao.Data);
            }
            catch (CustomException ex)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("listaSimulacoesPorProdutoEDia")]
        [SwaggerOperation(
            Summary = "Lista Simulações por Produto e Dia",
            Description = "Lista as simulações por produto e dia"
        )]
        [SwaggerResponse(200, "Retorna lista de simulações por produto e dia")]
        [SwaggerResponse(400, "Erro na requisição.")]
        [SwaggerResponse(500, "Erro interno do servidor.")]
        public async Task<IActionResult> ListaSimulacoesPorProdutoEDia(DateOnly dataReferencia, TipoSimulacaoEnum? tipoSimulacao, CancellationToken cancellationToken)
        {
            try
            {
                var lsSimulacao = await _simulacaoService.ListaSimulacoesPorProdutoEDia(dataReferencia, tipoSimulacao.ToString());

                if (lsSimulacao.StatusCode != StatusCodes.Status200OK)
                {
                    return StatusCode(lsSimulacao.StatusCode, lsSimulacao);
                }

                return StatusCode(lsSimulacao.StatusCode, lsSimulacao.Data);
            }
            catch (CustomException ex)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

    }
}
