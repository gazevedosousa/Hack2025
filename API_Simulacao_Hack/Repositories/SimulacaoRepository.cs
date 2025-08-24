using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.Interfaces.Repositories;
using API_Simulacao_Hack.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<long> BuscaQtdRegistros(int tipoSimulacao)
        {
            return await _simulacaoContext.Simulacoes
                .Where(s => s.TipoSimulacao == tipoSimulacao)
                .CountAsync();
        }

        public async Task<List<RetornoListaSimulacaoDTO>> ListaSimulacoesPaginadas(int pagina, int qtdRegistrosPagina, int tipoSimulacao)
        {
            return await _simulacaoContext.Simulacoes.Where(s => s.TipoSimulacao == tipoSimulacao)
                .Skip((pagina - 1) * qtdRegistrosPagina)
                .Take(qtdRegistrosPagina).AsNoTracking()
                .Select(s => new RetornoListaSimulacaoDTO
                {
                    IdSimulacao = s.IdSimulacao,
                    ValorDesejado = Math.Round(s.ValorDesejado, 2),
                    Prazo = s.Prazo,
                    ValorTotalParcelas = Math.Round(s.ValorTotalParcelas, 2)
                })
                .ToListAsync();
        }

        public async Task<List<Simulacao>> ListaSimulacoesPorDia(DateOnly dataReferencia, int tipoSimulacao)
        {
            return await _simulacaoContext.Simulacoes
                .Where(s => s.TipoSimulacao == tipoSimulacao && s.DataReferencia == dataReferencia)
                .OrderBy(s => s.CodigoProduto)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> ContaSimulacoesPorData(DateOnly dataReferencia)
        {
            int totalSimulacoes = await _simulacaoContext.Simulacoes
                .CountAsync(s => s.DataReferencia == dataReferencia);

            return totalSimulacoes / 2;

        }

    }
}
