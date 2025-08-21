using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.Models;
using API_Simulacao_Hack.Util.Base;
using API_Simulacao_Hack.Wrappers.Response;

namespace API_Simulacao_Hack.Interfaces.Services
{
    public interface ISimulacaoService
    {
        Task<ApiResponse<RetornoSimulacaoDTO>> RealizaSimulacao(SolicitacaoSimulacaoDTO simulacaoDTO);
        Task<ApiResponse<ResponsePaged<RetornoListaSimulacaoDTO>>> ListaSimulacoes(int pagina, int qtdRegistrosPagina);
        Task<ApiResponse<RetornoListaProdutoDiaDTO>> ListaSimulacoesPorProdutoEDia(DateOnly dataReferencia);
        Task<int> GeraIdSimulacao();
    }
}
