using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.Interfaces.Repositories;
using API_Simulacao_Hack.Models;
using API_Simulacao_Hack.Services;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace API_Simulacao_Hack.Repositories
{
    public class SimulacaoRepository : ISimulacaoRepository
    {
        private readonly DbHack _dbHack;
        private readonly SimulacaoContext _simulacaoContext;

        public SimulacaoRepository(DbHack dbHack, SimulacaoContext simulacaoContext)
        {
            _dbHack = dbHack;
            _simulacaoContext = simulacaoContext;
        }
        public async Task<Produto?> BuscaProduto(SolicitacaoSimulacaoDTO simulacaoDTO)
        {
            return await _dbHack.Produtos
                .Where(p => simulacaoDTO.ValorDesejado >= p.VrMinimo && (simulacaoDTO.ValorDesejado <= p.VrMaximo  || p.VrMaximo == null))
                .Where(p => simulacaoDTO.Prazo >= p.NuMinimoMeses && (simulacaoDTO.Prazo <= p.NuMaximoMeses || p.NuMaximoMeses == null))
                .FirstOrDefaultAsync();

        }

        public async Task<bool> SalvarSimulacao(Simulacao simulacao)
        {
            _simulacaoContext.Add(simulacao);

            var retornoBanco = await _simulacaoContext.SaveChangesAsync();
            return retornoBanco == 1;
        }

        public DbSet<Simulacao> MontaConsultaTotal()
        {
            return _simulacaoContext.Simulacoes;
        }

        public async Task<List<Simulacao>> ListaSimulacoesPaginadas(DbSet<Simulacao> query, int pagina, int qtdRegistrosPagina)
        {
            return await query.Skip((pagina - 1) * qtdRegistrosPagina)
                .Take(qtdRegistrosPagina).AsNoTracking()
                .Select(s => new Simulacao
                {
                    IdSimulacao = s.IdSimulacao,
                    ValorDesejado = s.ValorDesejado,
                    Prazo = s.Prazo,
                    ValorTotalParcelas = s.ValorTotalParcelas,
                    CodigoProduto = s.CodigoProduto,
                    DescricaoProduto = s.DescricaoProduto,
                    DataReferencia = s.DataReferencia,
                    TaxaJuros = s.TaxaJuros
                })
                .ToListAsync();
        }

        public async Task<List<Simulacao>> ListaSimulacoesPorDia(DateOnly dataReferencia)
        {
            return await _simulacaoContext.Simulacoes
                .Where(s => s.DataReferencia == dataReferencia)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
