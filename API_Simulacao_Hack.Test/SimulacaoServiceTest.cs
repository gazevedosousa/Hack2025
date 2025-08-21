using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.DTO.EventHub;
using API_Simulacao_Hack.Interfaces.Repositories;
using API_Simulacao_Hack.Interfaces.Services;
using API_Simulacao_Hack.Models;
using API_Simulacao_Hack.Repositories;
using API_Simulacao_Hack.Services;
using API_Simulacao_Hack.Util;
using API_Simulacao_Hack.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace API_Simulacao_Hack.Test
{
    public class SimulacaoServiceTest
    {

        private readonly Mock<ILogger<SimulacaoService>> _loggerMock;
        private readonly Mock<ISimulacaoRepository> _simulacaoRepositoryMock;
        private readonly Mock<IEventHubService> _eventHubServiceMock;
        private readonly Mock<ICalculoService> _calculoServiceMock;
        private readonly SolicitacaoSimulacaoValidator _validador;

        private readonly SimulacaoService _simulacaoService;

        public SimulacaoServiceTest()
        {
            _loggerMock = new Mock<ILogger<SimulacaoService>>();
            _simulacaoRepositoryMock = new Mock<ISimulacaoRepository>();
            _eventHubServiceMock = new Mock<IEventHubService>();
            _calculoServiceMock = new Mock<ICalculoService>();
            _validador = new SolicitacaoSimulacaoValidator();

            _simulacaoService = new SimulacaoService(
                _loggerMock.Object,
                _eventHubServiceMock.Object,    
                _simulacaoRepositoryMock.Object,
                _calculoServiceMock.Object,
                _validador);
        }

        #region BuscaListaProdutoDia

        [Fact]
        public async Task ListaSimulacoesPorProdutoEDia_Sucesso()
        {
            DateOnly dataReferencia = DateOnly.FromDateTime(DateTime.Now);

            var lsSimulacoes = new List<Simulacao>
            {
                new Simulacao
                {
                    IdSimulacao = 1,
                    ValorDesejado = 1000,
                    Prazo = 12,
                    ValorTotalParcelas = 1200,
                    CodigoProduto = 1,
                    DescricaoProduto = "Produto 1",
                    DataReferencia = dataReferencia,
                    TaxaJuros = 0.0179M
                },
                new Simulacao
                {
                    IdSimulacao = 2,
                    ValorDesejado = 2000,
                    Prazo = 12,
                    ValorTotalParcelas = 2400,
                    CodigoProduto = 1,
                    DescricaoProduto = "Produto 1",
                    DataReferencia = dataReferencia,
                    TaxaJuros = 0.0179M
                },
                new Simulacao
                {
                    IdSimulacao = 3,
                    ValorDesejado = 8500,
                    Prazo = 24,
                    ValorTotalParcelas = 10000,
                    CodigoProduto = 2,
                    DescricaoProduto = "Produto 2",
                    DataReferencia = dataReferencia,
                    TaxaJuros = 0.0160M
                },
                new Simulacao
                {
                    IdSimulacao = 4,
                    ValorDesejado = 10500,
                    Prazo = 12,
                    ValorTotalParcelas = 15000,
                    CodigoProduto = 2,
                    DescricaoProduto = "Produto 2",
                    DataReferencia = dataReferencia,
                    TaxaJuros = 0.0160M
                },
            };

            _simulacaoRepositoryMock
                .Setup(repo => repo.ListaSimulacoesPorDia(dataReferencia))
                .ReturnsAsync(lsSimulacoes.ToList());

            // Act
            var result = await _simulacaoService.ListaSimulacoesPorProdutoEDia(dataReferencia);

            // Assert
            Assert.NotEmpty(result.Data!.Simulacoes);
        }

        [Fact]
        public async Task ListaSimulacoesPorProdutoEDia_ErroDataReferencia()
        {
            DateOnly dataReferencia = default;

            // Act
            var result = await _simulacaoService.ListaSimulacoesPorProdutoEDia(dataReferencia);

            // Assert
            Assert.Equal("Informe a data de referência.", result.ErrorMessage);
        }

        #endregion

        #region BuscaListaPaginada

        [Fact]
        public async Task ListaSimulacoes_Sucesso()
        {
            int pagina = 1;
            int qtdRegistrosPagina = 1;

            var lsSimulacoes = new List<Simulacao>
            {
                new Simulacao
                {
                    IdSimulacao = 1,
                    ValorDesejado = 1000,
                    Prazo = 12,
                    ValorTotalParcelas = 1200
                },
                new Simulacao
                {
                    IdSimulacao = 2,
                    ValorDesejado = 8500,
                    Prazo = 24,
                    ValorTotalParcelas = 10000
                },
            };

            var queryableSimulacoes = lsSimulacoes.AsQueryable();

            var dbSetMock = new Mock<DbSet<Simulacao>>();
            dbSetMock.As<IQueryable<Simulacao>>().Setup(m => m.Provider).Returns(queryableSimulacoes.Provider);
            dbSetMock.As<IQueryable<Simulacao>>().Setup(m => m.Expression).Returns(queryableSimulacoes.Expression);
            dbSetMock.As<IQueryable<Simulacao>>().Setup(m => m.ElementType).Returns(queryableSimulacoes.ElementType);
            dbSetMock.As<IQueryable<Simulacao>>().Setup(m => m.GetEnumerator()).Returns(queryableSimulacoes.GetEnumerator());

            _simulacaoRepositoryMock
                .Setup(repo => repo.MontaConsultaTotal())
                .Returns(dbSetMock.Object);

            // Simule o mapeamento para DTOs
            var lsRetornoDTO = lsSimulacoes
                .Take(qtdRegistrosPagina)
                .Select(s => new RetornoListaSimulacaoDTO
                {
                    IdSimulacao = s.IdSimulacao,
                    ValorDesejado = s.ValorDesejado,
                    Prazo = s.Prazo,
                    ValorTotalParcelas = s.ValorTotalParcelas
                }).ToList();

            _simulacaoRepositoryMock
                .Setup(repo => repo.ListaSimulacoesPaginadas(dbSetMock.Object, pagina, qtdRegistrosPagina))
                .ReturnsAsync(lsRetornoDTO);

            // Act
            var result = await _simulacaoService.ListaSimulacoes(pagina, qtdRegistrosPagina);

            // Assert
            Assert.NotEmpty(result.Data!.registros);
        }

        #endregion

        #region RealizaSimulacao

        [Fact]
        public async Task RealizaSimulacao_ErroValorZerado()
        {
            SolicitacaoSimulacaoDTO solicitacao = new SolicitacaoSimulacaoDTO()
            {
                ValorDesejado = 0,
                Prazo = 12
            };

            // Act
            var result = await _simulacaoService.RealizaSimulacao(solicitacao);

            // Assert
            Assert.Equal("O valor desejado deve ser maior que zero.", result.ErrorMessage);
        }

        [Fact]
        public async Task RealizaSimulacao_ErroPrazoZerado()
        {
            SolicitacaoSimulacaoDTO solicitacao = new SolicitacaoSimulacaoDTO()
            {
                ValorDesejado = 1200,
                Prazo = 0
            };

            // Act
            var result = await _simulacaoService.RealizaSimulacao(solicitacao);

            // Assert
            Assert.Equal("O prazo deve ser maior que zero.", result.ErrorMessage);
        }

        [Fact]
        public async Task RealizaSimulacao_ErroProdutoNaoEncontrado()
        {
            SolicitacaoSimulacaoDTO solicitacao = new SolicitacaoSimulacaoDTO()
            {
                ValorDesejado = 250000,
                Prazo = 12
            };

            _simulacaoRepositoryMock
                .Setup(repo => repo.BuscaProduto(solicitacao))
                .ReturnsAsync((Produto?)null);

            // Act
            var result = await _simulacaoService.RealizaSimulacao(solicitacao);

            // Assert
            Assert.Equal("Produto não encontrado para o valor e prazo desejados.", result.ErrorMessage);
        }

        [Fact]
        public async Task RealizaSimulacao_ErroSalvamentoSimulacao()
        {
            SolicitacaoSimulacaoDTO solicitacao = new SolicitacaoSimulacaoDTO()
            {
                ValorDesejado = 1200,
                Prazo = 12
            };

            Produto produto = new Produto
            {
                CoProduto = 1,
                NoProduto = "Produto 1",
                PcTaxaJuros = 0.0179M,
                NuMinimoMeses = 0,
                NuMaximoMeses = 24,
                VrMinimo = 200,
                VrMaximo = 10000
            };

            List<ParcelasDTO> lsParcelas = new List<ParcelasDTO>()
            {
                new ParcelasDTO
                {
                    Numero = 1,
                    ValorAmortizacao = 100,
                    ValorJuros = 20,
                    ValorPrestacao = 120
                },
            };

            var retornoSimulacao = new RetornoSimulacaoDTO
            {
                ResultadosSimulacao = new List<ResultadoSimulacaoDTO>
                {
                    new ResultadoSimulacaoDTO { Tipo = "PRICE", Parcelas = lsParcelas },
                    new ResultadoSimulacaoDTO { Tipo = "SAC", Parcelas = lsParcelas }
                }
            };

            _simulacaoRepositoryMock
                .Setup(repo => repo.BuscaProduto(solicitacao))
                .ReturnsAsync(produto);

            _calculoServiceMock
                .Setup(repo => repo.CalculaParcelas(solicitacao, produto.PcTaxaJuros))
                .Returns(retornoSimulacao.ResultadosSimulacao);

            _simulacaoRepositoryMock
                .Setup(repo => repo.SalvarSimulacao(It.IsAny<Simulacao>()))
                .ReturnsAsync(false);

            // Act
            var result = await _simulacaoService.RealizaSimulacao(solicitacao);

            // Assert
            Assert.Equal("Erro ao salvar simulação no banco de dados", result.ErrorMessage);
        }

        [Fact]
        public async Task RealizaSimulacao_Sucesso()
        {
            SolicitacaoSimulacaoDTO solicitacao = new SolicitacaoSimulacaoDTO()
            {
                ValorDesejado = 1200,
                Prazo = 12
            };

            Produto produto = new Produto
            {
                CoProduto = 1,
                NoProduto = "Produto 1",
                PcTaxaJuros = 0.0179M,
                NuMinimoMeses = 0,
                NuMaximoMeses = 24,
                VrMinimo = 200,
                VrMaximo = 10000
            };

            List<ParcelasDTO> lsParcelas = new List<ParcelasDTO>()
            {
                new ParcelasDTO
                {
                    Numero = 1,
                    ValorAmortizacao = 100,
                    ValorJuros = 20,
                    ValorPrestacao = 120
                },
            };

            var retornoSimulacao = new RetornoSimulacaoDTO
            {
                ResultadosSimulacao = new List<ResultadoSimulacaoDTO>
                {
                    new ResultadoSimulacaoDTO { Tipo = "PRICE", Parcelas = lsParcelas },
                    new ResultadoSimulacaoDTO { Tipo = "SAC", Parcelas = lsParcelas }
                }
            };

            _simulacaoRepositoryMock
                .Setup(repo => repo.BuscaProduto(solicitacao))
                .ReturnsAsync(produto);

            _calculoServiceMock
                .Setup(repo => repo.CalculaParcelas(solicitacao, produto.PcTaxaJuros))
                .Returns(retornoSimulacao.ResultadosSimulacao);

            _simulacaoRepositoryMock
                .Setup(repo => repo.SalvarSimulacao(It.IsAny<Simulacao>()))
                .ReturnsAsync(true);

            _eventHubServiceMock
                .Setup(repo => repo.EnviaEvento(It.IsAny<EventHubDTO>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _simulacaoService.RealizaSimulacao(solicitacao);

            // Assert
            Assert.NotEmpty(result.Data!.ResultadosSimulacao);
        }

        #endregion

        #region GeraIdSimulacao

        [Fact]
        public async Task GeraIdSimulacao_DeveRetornarIdCorreto_QuandoNaoExistemSimulacoes()
        {
            // Arrange
            var mockRepo = new Mock<ISimulacaoRepository>();
            mockRepo.Setup(r => r.ContaSimulacoesPorDataAsync(It.IsAny<DateOnly>())).ReturnsAsync(0);

            string dataReferencia = DateTime.Now.ToString("yyyyMMdd");
            int esperado = int.Parse($"{dataReferencia}1");

            // Act
            int resultado = await _simulacaoService.GeraIdSimulacao();

            // Assert
            Assert.Equal(esperado, resultado);
        }

        [Fact]
        public async Task GeraIdSimulacao_DeveRetornarIdCorreto_QuandoExistemSimulacoes()
        {
            // Arrange
            _simulacaoRepositoryMock.Setup(r => r.ContaSimulacoesPorDataAsync(It.IsAny<DateOnly>())).ReturnsAsync(5);

            string dataReferencia = DateTime.Now.ToString("yyyyMMdd");
            int esperado = int.Parse($"{dataReferencia}6");

            // Act
            int resultado = await _simulacaoService.GeraIdSimulacao();

            // Assert
            Assert.Equal(esperado, resultado);
        }

        [Fact]
        public async Task RealizaSimulacao_DeveRetornarBadRequest_QuandoTipoSimulacaoDesconhecido()
        {
            SolicitacaoSimulacaoDTO solicitacao = new SolicitacaoSimulacaoDTO()
            {
                ValorDesejado = 1200,
                Prazo = 12
            };


            Produto produto = new Produto
            {
                CoProduto = 1,
                NoProduto = "Produto 1",
                PcTaxaJuros = 0.0179M,
                NuMinimoMeses = 0,
                NuMaximoMeses = 24,
                VrMinimo = 200,
                VrMaximo = 10000
            };

            _simulacaoRepositoryMock.Setup(r => r.BuscaProduto(solicitacao)).ReturnsAsync(produto);

            var retornoSimulacao = new RetornoSimulacaoDTO
            {
                ResultadosSimulacao = new List<ResultadoSimulacaoDTO>
                {
                    new ResultadoSimulacaoDTO { Tipo = "PRICE", Parcelas = new List<ParcelasDTO>() },
                    new ResultadoSimulacaoDTO { Tipo = "SAC", Parcelas = new List<ParcelasDTO>() }
                }
            };

            _calculoServiceMock.Setup(c => c.CalculaParcelas(solicitacao, produto.PcTaxaJuros)).Returns(retornoSimulacao.ResultadosSimulacao);

            // Act
            var result = await _simulacaoService.RealizaSimulacao(solicitacao);

            //Assert
            Assert.Equal("Erro ao salvar simulação no banco de dados", result.ErrorMessage);

        }

        #endregion
    }
}
