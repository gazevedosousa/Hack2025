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
        private readonly ICalculoService _calculoService;
        private readonly SolicitacaoSimulacaoValidator _validador;

        public SimulacaoService(ILogger<SimulacaoService> logger, IEventHubService eventHubService,
            ISimulacaoRepository simulacaoRepository, ICalculoService calculoService, SolicitacaoSimulacaoValidator validador)
        {
            _logger = logger;
            _eventHubService = eventHubService;
            _simulacaoRepository = simulacaoRepository;
            _calculoService = calculoService;
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

            List<ResultadoSimulacaoDTO> lsResultadoSimulacao = _calculoService.CalculaParcelas(simulacaoDTO, produto.PcTaxaJuros);

            retornoSimulacao.IdSimulacao = await GeraIdSimulacao();
            retornoSimulacao.CodigoProduto = produto.CoProduto;
            retornoSimulacao.DescricaoProduto = produto.NoProduto;
            retornoSimulacao.TaxaJuros = produto.PcTaxaJuros;
            retornoSimulacao.ResultadosSimulacao = lsResultadoSimulacao;
           
            decimal valorMediaPrestacoes = lsResultadoSimulacao
                .SelectMany(r => r.Parcelas)
                .Average(pr => pr.ValorPrestacao);

            // Salvar simulação no banco de dados
            Simulacao simulacao = new Simulacao
            {
                IdSimulacao = retornoSimulacao.IdSimulacao,
                ValorDesejado = simulacaoDTO.ValorDesejado,
                Prazo = simulacaoDTO.Prazo,
                ValorTotalParcelas = lsResultadoSimulacao.Where(s => s.Tipo == "SAC").SelectMany(s => s.Parcelas).Sum(p => p.ValorPrestacao),
                CodigoProduto = retornoSimulacao.CodigoProduto,
                DescricaoProduto = retornoSimulacao.DescricaoProduto,
                DataReferencia = DateOnly.FromDateTime(new DateTime().GetDataAtual().Date),
                TaxaJuros = retornoSimulacao.TaxaJuros,
                ValorMediaPrestacoes = valorMediaPrestacoes
            };

            if (!await _simulacaoRepository.SalvarSimulacao(simulacao))
            {
                _logger.LogError("Erro ao salvar simulação no banco de dados");
                return ApiResponse<RetornoSimulacaoDTO>.BadRequest(retornoSimulacao, "Erro ao salvar simulação no banco de dados");
            }

            await _eventHubService.EnviaEvento(new EventHubDTO()
            {
                SolicitacaoSimulacao = simulacaoDTO,
                RetornoSimulacao = retornoSimulacao
            });

            return ApiResponse<RetornoSimulacaoDTO>.SuccessOK(retornoSimulacao);
        }

        public async Task<ApiResponse<ResponsePaged<RetornoListaSimulacaoDTO>>> ListaSimulacoes(int pagina, int qtdRegistrosPagina)
        {
            pagina = pagina == 0 ? 1 : pagina;
            qtdRegistrosPagina = qtdRegistrosPagina == 0 ? 1 : qtdRegistrosPagina;

            DbSet<Simulacao> query = _simulacaoRepository.MontaConsultaTotal();

            long qtdRegistrosTotal = query.Count();

            List<RetornoListaSimulacaoDTO> lsSimulacoes = await _simulacaoRepository.ListaSimulacoesPaginadas(query, pagina, qtdRegistrosPagina);

            ResponsePaged<RetornoListaSimulacaoDTO> responsePaged = new ResponsePaged<RetornoListaSimulacaoDTO>(lsSimulacoes, pagina, lsSimulacoes.Count(), qtdRegistrosTotal);

            return ApiResponse<ResponsePaged<RetornoListaSimulacaoDTO>>.SuccessOK(responsePaged);
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
                        TaxaMediaJuro = g.Sum(s => s.TaxaJuros) / g.Count(),
                        ValorTotalDesejado = Math.Round(g.Sum(s => s.ValorDesejado), 2),
                        ValorTotalCredito = Math.Round(g.Sum(s => s.ValorTotalParcelas), 2),
                        ValorMedioPrestacao = Math.Round(g.Sum(s => s.ValorMediaPrestacoes) / g.Count(), 2)
                    })
                    .ToList();

                retornoListaProdutoDia.Simulacoes = agrupadoPorProduto;
            }

            return ApiResponse<RetornoListaProdutoDiaDTO>.SuccessOK(retornoListaProdutoDia);
        }

        public async Task<int> GeraIdSimulacao()
        {
            string dataReferencia = new DateTime().GetDataAtual().Date.ToString("yyyyMMdd");

            int sequencial = await _simulacaoRepository.ContaSimulacoesPorData(DateOnly.FromDateTime(new DateTime().GetDataAtual().Date));

            sequencial++;

            return int.Parse($"{dataReferencia}{sequencial}");
        }
    }
}

