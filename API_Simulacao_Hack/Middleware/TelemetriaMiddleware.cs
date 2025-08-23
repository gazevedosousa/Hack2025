using API_Simulacao_Hack.Interfaces.Services;
using System.Diagnostics.CodeAnalysis;

namespace API_Simulacao_Hack.Middleware
{
    [ExcludeFromCodeCoverage]
    public class TelemetriaMiddleware
    {
        private readonly RequestDelegate _next;

        public TelemetriaMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITelemetriaService telemetriaService)
        {
            var nomeApi = context.Request.Path.Value ?? "Desconhecido";
            var inicio = DateTime.UtcNow;
            bool sucesso = false;

            try
            {
                await _next(context);
                sucesso = context.Response.StatusCode < 400;
            }
            finally
            {
                var tempoExecucao = (DateTime.UtcNow - inicio).TotalMilliseconds;
                telemetriaService.RegistrarRequisicao(nomeApi, tempoExecucao, sucesso);
            }
        }
    }
}
