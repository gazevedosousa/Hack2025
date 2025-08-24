using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Simulacao_Hack.Interfaces.Repositories
{
    public interface ISimulacaoRepository
    {
        Task<Produto?> BuscaProduto(SolicitacaoSimulacaoDTO simulacaoDTO);
        Task<bool> SalvarSimulacao(Simulacao simulacao);
        DbSet<Simulacao> MontaConsultaTotal();
        Task<List<RetornoListaSimulacaoDTO>> ListaSimulacoesPaginadas(DbSet<Simulacao> query, int pagina, int qtdRegistrosPagina, string tipoSimulacao);
        Task<List<Simulacao>> ListaSimulacoesPorDia(DateOnly dataReferencia, string tipoSimulacao);
        Task<int> ContaSimulacoesPorData(DateOnly dataReferencia);
    }
}
