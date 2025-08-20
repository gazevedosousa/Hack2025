using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.Models;
using API_Simulacao_Hack.Util.Base;
using API_Simulacao_Hack.Wrappers.Response;

namespace API_Simulacao_Hack.Interfaces.Services
{
    public interface ITelemetriaService
    {
        void RegistrarRequisicao(string nomeApi, double tempoExecucao, bool sucesso);
        ApiResponse<RetornoTelemetriaDTO> ObterTodasTelemetrias();
    }
}
