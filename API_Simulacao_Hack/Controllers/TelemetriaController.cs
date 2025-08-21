using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using API_Simulacao_Hack.Util.Base;
using API_Simulacao_Hack.Interfaces.Services;

namespace API_Simulacao_Hack.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class TelemetriaController : ControllerBase
    {
        private readonly ITelemetriaService _telemetriaService;

        public TelemetriaController(ITelemetriaService telemetriaService)
        {
            _telemetriaService = telemetriaService;
        }

        [HttpGet]
        [Route("dadosTelemetria")]
        [SwaggerOperation(
            Summary = "Dados de Telemetria",
            Description = "Lista os dados de telemetria"
        )]
        [SwaggerResponse(200, "Retorna dados telemetria")]
        [SwaggerResponse(500, "Erro interno do servidor.")]
        public IActionResult ListaTelemetria(CancellationToken cancellationToken)
        {
            try
            {
                var lsTelemetria = _telemetriaService.ObterTodasTelemetrias();

                if (lsTelemetria.StatusCode != StatusCodes.Status200OK)
                {
                    return StatusCode(lsTelemetria.StatusCode, lsTelemetria.ErrorMessage);
                }

                return StatusCode(lsTelemetria.StatusCode, lsTelemetria.Data);
            }
            catch (CustomException ex)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

    }
}
