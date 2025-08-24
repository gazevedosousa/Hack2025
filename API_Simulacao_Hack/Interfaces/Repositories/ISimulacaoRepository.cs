using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Simulacao_Hack.Interfaces.Repositories
{
    public interface ISimulacaoRepository
    {
        Task<Produto?> BuscaProduto(SolicitacaoSimulacaoDTO simulacaoDTO);
        Task<bool> SalvarSimulacao(Simulacao simulacao);
        Task<long> BuscaQtdRegistros(int tipoSimulacao);
        Task<List<RetornoListaSimulacaoDTO>> ListaSimulacoesPaginadas(int pagina, int qtdRegistrosPagina, int tipoSimulacao);
        Task<List<Simulacao>> ListaSimulacoesPorDia(DateOnly dataReferencia, int tipoSimulacao);
        Task<int> ContaSimulacoesPorData(DateOnly dataReferencia);
    }
}
