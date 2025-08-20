using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.DTO.EventHub;
using API_Simulacao_Hack.Models;
using API_Simulacao_Hack.Util;
using API_Simulacao_Hack.Util.Base;
using Microsoft.EntityFrameworkCore;
using API_Simulacao_Hack.Interfaces.Services;
using API_Simulacao_Hack.Interfaces.Repositories;
using API_Simulacao_Hack.Wrappers.Response;
using API_Simulacao_Hack.Validators;
using FluentValidation.Results;

namespace API_Simulacao_Hack.Services
{
    public class SimulacaoService : ISimulacaoService
    {
        private readonly ILogger<SimulacaoService> _logger;
        private readonly IEventHubService _eventHubService;
        private readonly ISimulacaoRepository _simulacaoRepository;
        private readonly SolicitacaoSimulacaoValidator _validador;

        public SimulacaoService(ILogger<SimulacaoService> logger, IEventHubService eventHubService, ISimulacaoRepository simulacaoRepository, SolicitacaoSimulacaoValidator validador)
        {
            _logger = logger;
            _eventHubService = eventHubService;
            _simulacaoRepository = simulacaoRepository;
            _validador = validador;
        }

        public async Task<ApiResponse<RetornoSimulacaoDTO>> RealizaSimulacao(SolicitacaoSimulacaoDTO simulacaoDTO)
        {
            
            RetornoSimulacaoDTO retornoSimulacao = new RetornoSimulacaoDTO();

            ValidationResult validacao = _validador.Validate(simulacaoDTO);

            if (!validacao.IsValid)
            {
                _logger.LogError("Solicitação de simulação inválida. ValorDesejado: {ValorDesejado}, Prazo: {Prazo}", simulacaoDTO.ValorDesejado, simulacaoDTO.Prazo);
                return ApiResponse<RetornoSimulacaoDTO>.BadRequest(retornoSimulacao, validacao.Errors[0].ErrorMessage);
            }

            Produto? produto = await _simulacaoRepository.BuscaProduto(simulacaoDTO);

            if (produto == null)
            {
                _logger.LogError("Produto não encontrado para o valor e prazo desejados. Valor:{ValorDesejado}, Prazo: {Prazo}", simulacaoDTO.ValorDesejado, simulacaoDTO.Prazo);
                return ApiResponse<RetornoSimulacaoDTO>.NotFound(retornoSimulacao, "Produto não encontrado para o valor e prazo desejados.");
            }

            retornoSimulacao.CodigoProduto = produto.CoProduto;
            retornoSimulacao.DescricaoProduto = produto.NoProduto;
            retornoSimulacao.TaxaJuros = produto.PcTaxaJuros;
            retornoSimulacao.ResultadosSimulacao = CalculoUtil.CalculaParcelas(simulacaoDTO, produto.PcTaxaJuros);

            // Salvar simulação no banco de dados
            Simulacao simulacao = new Simulacao
            {
                ValorDesejado = simulacaoDTO.ValorDesejado,
                Prazo = simulacaoDTO.Prazo,
                ValorTotalParcelas = retornoSimulacao.ResultadosSimulacao
                    .Where(r => r.Tipo == "SAC")
                    .Sum(r => r.Parcelas.Sum(p => p.ValorPrestacao)),
                CodigoProduto = retornoSimulacao.CodigoProduto,
                DescricaoProduto = retornoSimulacao.DescricaoProduto,
                DataReferencia = DateOnly.FromDateTime(new DateTime().GetDataAtual().Date),
                TaxaJuros = retornoSimulacao.TaxaJuros
            };

            if (!await _simulacaoRepository.SalvarSimulacao(simulacao))
            {
                _logger.LogError("Erro ao salvar simulação no banco de dados");
                return ApiResponse<RetornoSimulacaoDTO>.BadRequest(retornoSimulacao, "Erro ao salvar simulação no banco de dados");
            }

            retornoSimulacao.IdSimulacao = simulacao.IdSimulacao;

            await _eventHubService.EnviaEvento(new EventHubDTO()
            {
                SolicitacaoSimulacao = simulacaoDTO,
                RetornoSimulacao = retornoSimulacao
            });

            return ApiResponse<RetornoSimulacaoDTO>.SuccessOK(retornoSimulacao);
        }

        public async Task<ApiResponse<ResponsePaged<Simulacao>>> ListaSimulacoes(int pagina, int qtdRegistrosPagina)
        {
            pagina = pagina == 0 ? 1 : pagina;
            qtdRegistrosPagina = qtdRegistrosPagina == 0 ? 1 : qtdRegistrosPagina;

            DbSet<Simulacao> query = _simulacaoRepository.MontaConsultaTotal();

            long qtdRegistrosTotal = query.Count();

            List<Simulacao> lsSimulacoes = await _simulacaoRepository.ListaSimulacoesPaginadas(query, pagina, qtdRegistrosPagina);

            ResponsePaged<Simulacao> responsePaged = new ResponsePaged<Simulacao>(lsSimulacoes, pagina, lsSimulacoes.Count(), qtdRegistrosTotal);

            return ApiResponse<ResponsePaged<Simulacao>>.SuccessOK(responsePaged);
        }

        public async Task<ApiResponse<RetornoListaProdutoDiaDTO>> ListaSimulacoesPorProdutoEDia(DateOnly dataReferencia)
        {
            RetornoListaProdutoDiaDTO retornoListaProdutoDia = new RetornoListaProdutoDiaDTO();

            if(dataReferencia == default)
            {
                _logger.LogError("Data de Referência inválida");
                return ApiResponse<RetornoListaProdutoDiaDTO>.BadRequest(retornoListaProdutoDia, "Informe a data de referência.");
            }

            retornoListaProdutoDia.DataReferencia = dataReferencia;

            List<Simulacao> lsSimulacoes = await _simulacaoRepository.ListaSimulacoesPorDia(dataReferencia);

            if (lsSimulacoes.Count > 0)
            {
                var agrupadoPorProduto = lsSimulacoes
                    .GroupBy(s => s.CodigoProduto)
                    .Select(g => new RetornoSimulacoesProdutoDiaDTO
                    {
                        CodigoProduto = g.First().CodigoProduto,
                        DescricaoProduto = g.First().DescricaoProduto,
                        TaxaMediaJuro = g.First().TaxaJuros,
                        ValorTotalDesejado = g.Sum(s => s.ValorDesejado),
                        ValorTotalCredito = g.Sum(s => s.ValorTotalParcelas),
                        ValorMedioPrestacao = g.Sum(s => s.ValorTotalParcelas) / (g.Sum(s => s.Prazo) == 0 ? 1 : g.Sum(s => s.Prazo))
                    })
                    .ToList();

                retornoListaProdutoDia.Simulacoes = agrupadoPorProduto;
            }

            return ApiResponse<RetornoListaProdutoDiaDTO>.SuccessOK(retornoListaProdutoDia);
        }
    }
}

