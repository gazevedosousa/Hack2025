using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.Models;
using API_Simulacao_Hack.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace API_Simulacao_Hack.Test.Repositories
{
    public class SimulacaoRepositoryTest
    {

        [Fact]
        public async Task BuscaProduto_Sucesso()
        {
            using var dbHackInMemory = SimulacaoRepositoryFixture.GetInMemoryContextDbHackContext();
            using var dbSimulacaoContextInMemory = SimulacaoRepositoryFixture.GetInMemoryContextSimulacaoContext();

            var _dbHack = dbHackInMemory;
            var _simulacaoContext = dbSimulacaoContextInMemory;

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(_dbHack, _simulacaoContext);

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

            _dbHack.Add(produto);
            _dbHack.SaveChanges();

            SolicitacaoSimulacaoDTO solicitacao = new SolicitacaoSimulacaoDTO()
            {
                ValorDesejado = 1200,
                Prazo = 12
            };

            // Act
            var result = await simulacaoRepository.BuscaProduto(solicitacao);

            // Assert
            Assert.Equal(1, result!.CoProduto);
            Assert.Equal("Produto 1", result!.NoProduto);
            Assert.Equal(0.0179M, result!.PcTaxaJuros);
            Assert.Equal(0, result!.NuMinimoMeses);
            Assert.Equal(24, (short)result!.NuMaximoMeses!);
            Assert.Equal(200, result!.VrMinimo);
            Assert.Equal(10000, result!.VrMaximo);
        }

        [Fact]
        public async Task BuscaProduto_Zerado()
        {
            using var dbHackInMemory = SimulacaoRepositoryFixture.GetInMemoryContextDbHackContext();
            using var dbSimulacaoContextInMemory = SimulacaoRepositoryFixture.GetInMemoryContextSimulacaoContext();

            var _dbHack = dbHackInMemory;
            var _simulacaoContext = dbSimulacaoContextInMemory;

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(_dbHack, _simulacaoContext);

            SolicitacaoSimulacaoDTO solicitacao = new SolicitacaoSimulacaoDTO()
            {
                ValorDesejado = 12000,
                Prazo = 12
            };

            // Act
            var result = await simulacaoRepository.BuscaProduto(solicitacao);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SalvaSimulacao_Sucesso()
        {

            using var dbHackInMemory = SimulacaoRepositoryFixture.GetInMemoryContextDbHackContext();
            using var dbSimulacaoContextInMemory = SimulacaoRepositoryFixture.GetInMemoryContextSimulacaoContext();

            var _dbHack = dbHackInMemory;
            var _simulacaoContext = dbSimulacaoContextInMemory;

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(_dbHack, _simulacaoContext);

            Simulacao simulacao = new Simulacao()
            {
                IdSimulacao = 1,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1339.62M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 111.64M,
                TipoSimulacao = 1
            };

            // Act
            var result = await simulacaoRepository.SalvarSimulacao(simulacao);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SalvarSimulacao_Erro()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SimulacaoContext>()
                .Options;

            var mockContext = new Mock<SimulacaoContext>(options);
            mockContext.Setup(c => c.SaveChangesAsync(default))
                       .ReturnsAsync(0);

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(null!, mockContext.Object);

            Simulacao simulacao = new Simulacao()
            {
                IdSimulacao = 1,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1339.62M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 111.64M,
                TipoSimulacao = 1
            };

            // Act
            var result = await simulacaoRepository.SalvarSimulacao(simulacao);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task BuscaQtdRegistros_Sucesso()
        {

            using var dbHackInMemory = SimulacaoRepositoryFixture.GetInMemoryContextDbHackContext();
            using var dbSimulacaoContextInMemory = SimulacaoRepositoryFixture.GetInMemoryContextSimulacaoContext();

            var _dbHack = dbHackInMemory;
            var _simulacaoContext = dbSimulacaoContextInMemory;

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(_dbHack, _simulacaoContext);

            Simulacao simulacao = new Simulacao()
            {
                IdSimulacao = 1,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1339.62M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 111.64M,
                TipoSimulacao = 1
            };

            _simulacaoContext.Add(simulacao);
            _simulacaoContext.SaveChanges();

            // Act
            long result = await simulacaoRepository.BuscaQtdRegistros(1);

            // Assert
            Assert.Equal(1L, result);
        }

        [Fact]
        public async Task BuscaQtdRegistros_Vazia()
        {
            using var dbHackInMemory = SimulacaoRepositoryFixture.GetInMemoryContextDbHackContext();
            using var dbSimulacaoContextInMemory = SimulacaoRepositoryFixture.GetInMemoryContextSimulacaoContext();

            var _dbHack = dbHackInMemory;
            var _simulacaoContext = dbSimulacaoContextInMemory;

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(_dbHack, _simulacaoContext);

            // Act
            long result = await simulacaoRepository.BuscaQtdRegistros(1);

            // Assert
            Assert.Equal(0L, result);
        }

        [Fact]
        public async Task ListaSimulacoesPaginadas_DeveRetornarListaVazia()
        {

            using var dbHackInMemory = SimulacaoRepositoryFixture.GetInMemoryContextDbHackContext();
            using var dbSimulacaoContextInMemory = SimulacaoRepositoryFixture.GetInMemoryContextSimulacaoContext();

            var _dbHack = dbHackInMemory;
            var _simulacaoContext = dbSimulacaoContextInMemory;

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(_dbHack, _simulacaoContext);

            DbSet<Simulacao> dbSet = _simulacaoContext.Simulacoes;

            int pagina = 1;
            int qtdRegistrosPagina = 1;
            int tipoSimulacao = 1;

            // Act
            var result = await simulacaoRepository.ListaSimulacoesPaginadas(pagina, qtdRegistrosPagina, tipoSimulacao);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ListaSimulacoesPaginadas_DeveRetornarUmRegistroApenas()
        {

            using var dbHackInMemory = SimulacaoRepositoryFixture.GetInMemoryContextDbHackContext();
            using var dbSimulacaoContextInMemory = SimulacaoRepositoryFixture.GetInMemoryContextSimulacaoContext();

            var _dbHack = dbHackInMemory;
            var _simulacaoContext = dbSimulacaoContextInMemory;

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(_dbHack, _simulacaoContext);

            Simulacao simulacaoSAC = new Simulacao()
            {
                IdSimulacao = 1,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1339.62M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 111.64M,
                TipoSimulacao = 1
            };

            Simulacao simulacaoPRICE = new Simulacao()
            {
                IdSimulacao = 1,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1344.12M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 112.01M,
                TipoSimulacao = 2
            };

            _simulacaoContext.Add(simulacaoSAC);
            _simulacaoContext.Add(simulacaoPRICE);
            _simulacaoContext.SaveChanges();

            int pagina = 1;
            int qtdRegistrosPagina = 1;

            int tipoSimulacao = 1;

            // Act
            var result = await simulacaoRepository.ListaSimulacoesPaginadas(pagina, qtdRegistrosPagina, tipoSimulacao);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result.First().IdSimulacao);
            Assert.Equal(1200, result.First().ValorDesejado);
        }

        [Fact]
        public async Task ListaSimulacoesPaginadas_DeveRetornarDoisRegistros()
        {

            using var dbHackInMemory = SimulacaoRepositoryFixture.GetInMemoryContextDbHackContext();
            using var dbSimulacaoContextInMemory = SimulacaoRepositoryFixture.GetInMemoryContextSimulacaoContext();

            var _dbHack = dbHackInMemory;
            var _simulacaoContext = dbSimulacaoContextInMemory;

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(_dbHack, _simulacaoContext);

            Simulacao simulacaoSAC1 = new Simulacao()
            {
                IdSimulacao = 1,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1339.62M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 111.64M,
                TipoSimulacao = 1
            };

            Simulacao simulacaoSAC2 = new Simulacao()
            {
                IdSimulacao = 2,
                ValorDesejado = 25000,
                Prazo = 48,
                ValorTotalParcelas = 35718.76M,
                CodigoProduto = 2,
                DescricaoProduto = "Produto 2",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0175M,
                ValorMediaPrestacoes = 744.14M,
                TipoSimulacao = 1
            };

            _simulacaoContext.Add(simulacaoSAC1);
            _simulacaoContext.Add(simulacaoSAC2);
            _simulacaoContext.SaveChanges();

            int pagina = 1;
            int qtdRegistrosPagina = 2;
            int tipoSimulacao = 1;

            // Act
            var result = await simulacaoRepository.ListaSimulacoesPaginadas(pagina, qtdRegistrosPagina, tipoSimulacao);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.IdSimulacao == 1 && r.ValorDesejado == 1200);
            Assert.Contains(result, r => r.IdSimulacao == 2 && r.ValorDesejado == 25000);
        }

        [Fact]
        public async Task ListaSimulacoes_DeveRetornarListaVazia()
        {

            using var dbHackInMemory = SimulacaoRepositoryFixture.GetInMemoryContextDbHackContext();
            using var dbSimulacaoContextInMemory = SimulacaoRepositoryFixture.GetInMemoryContextSimulacaoContext();

            var _dbHack = dbHackInMemory;
            var _simulacaoContext = dbSimulacaoContextInMemory;

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(_dbHack, _simulacaoContext);

            DateOnly dataReferencia = DateOnly.FromDateTime(DateTime.Now);

            int tipoSimulacao = 1;

            // Act
            var result = await simulacaoRepository.ListaSimulacoesPorDia(dataReferencia, tipoSimulacao);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ListaSimulacoes_DeveRetornarUmRegistroApenas()
        {

            using var dbHackInMemory = SimulacaoRepositoryFixture.GetInMemoryContextDbHackContext();
            using var dbSimulacaoContextInMemory = SimulacaoRepositoryFixture.GetInMemoryContextSimulacaoContext();

            var _dbHack = dbHackInMemory;
            var _simulacaoContext = dbSimulacaoContextInMemory;

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(_dbHack, _simulacaoContext);

            Simulacao simulacaoSAC = new Simulacao()
            {
                IdSimulacao = 1,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1339.62M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 111.64M,
                TipoSimulacao = 1
            };

            Simulacao simulacaoPRICE = new Simulacao()
            {
                IdSimulacao = 1,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1344.12M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 112.01M,
                TipoSimulacao = 2
            };

            _simulacaoContext.Add(simulacaoSAC);
            _simulacaoContext.Add(simulacaoPRICE);
            _simulacaoContext.SaveChanges();

            DateOnly dataReferencia = DateOnly.FromDateTime(DateTime.Now);

            int tipoSimulacao = 1;


            // Act
            var result = await simulacaoRepository.ListaSimulacoesPorDia(dataReferencia, tipoSimulacao);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result.First().IdSimulacao);
            Assert.Equal(1200, result.First().ValorDesejado);
        }

        [Fact]
        public async Task ListaSimulacoes_DeveRetornarDoisRegistros()
        {

            using var dbHackInMemory = SimulacaoRepositoryFixture.GetInMemoryContextDbHackContext();
            using var dbSimulacaoContextInMemory = SimulacaoRepositoryFixture.GetInMemoryContextSimulacaoContext();

            var _dbHack = dbHackInMemory;
            var _simulacaoContext = dbSimulacaoContextInMemory;

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(_dbHack, _simulacaoContext);

            Simulacao simulacaoSAC1 = new Simulacao()
            {
                IdSimulacao = 1,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1339.62M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 111.64M,
                TipoSimulacao = 1
            };

            Simulacao simulacaoSAC2 = new Simulacao()
            {
                IdSimulacao = 2,
                ValorDesejado = 25000,
                Prazo = 48,
                ValorTotalParcelas = 35718.76M,
                CodigoProduto = 2,
                DescricaoProduto = "Produto 2",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0175M,
                ValorMediaPrestacoes = 744.14M,
                TipoSimulacao = 1
            };

            _simulacaoContext.Add(simulacaoSAC1);
            _simulacaoContext.Add(simulacaoSAC2);
            _simulacaoContext.SaveChanges();

            DateOnly dataReferencia = DateOnly.FromDateTime(DateTime.Now);

            int tipoSimulacao = 1;

            // Act
            var result = await simulacaoRepository.ListaSimulacoesPorDia(dataReferencia, tipoSimulacao);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.IdSimulacao == 1 && r.ValorDesejado == 1200);
            Assert.Contains(result, r => r.IdSimulacao == 2 && r.ValorDesejado == 25000);
        }

        [Fact]
        public async Task ContaSimulacoesPorData_DeveRetornarZero()
        {

            using var dbHackInMemory = SimulacaoRepositoryFixture.GetInMemoryContextDbHackContext();
            using var dbSimulacaoContextInMemory = SimulacaoRepositoryFixture.GetInMemoryContextSimulacaoContext();

            var _dbHack = dbHackInMemory;
            var _simulacaoContext = dbSimulacaoContextInMemory;

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(_dbHack, _simulacaoContext);

            DateOnly dataReferencia = DateOnly.FromDateTime(DateTime.Now);

            // Act
            var result = await simulacaoRepository.ContaSimulacoesPorData(dataReferencia);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task ContaSimulacoesPorData_DeveRetornarExatamenteUm()
        {

            using var dbHackInMemory = SimulacaoRepositoryFixture.GetInMemoryContextDbHackContext();
            using var dbSimulacaoContextInMemory = SimulacaoRepositoryFixture.GetInMemoryContextSimulacaoContext();

            var _dbHack = dbHackInMemory;
            var _simulacaoContext = dbSimulacaoContextInMemory;

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(_dbHack, _simulacaoContext);

            Simulacao simulacaoSAC1 = new Simulacao()
            {
                IdSimulacao = 1,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1339.62M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 111.64M,
                TipoSimulacao = 1
            };

            Simulacao simulacaoPRICE1 = new Simulacao()
            {
                IdSimulacao = 1,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1344.12M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 112.01M,
                TipoSimulacao = 2
            };

            Simulacao simulacaoSAC2 = new Simulacao()
            {
                IdSimulacao = 2,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1339.62M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(new DateTime(2025,01,01)),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 111.64M,
                TipoSimulacao = 1
            };

            Simulacao simulacaoPRICE2 = new Simulacao()
            {
                IdSimulacao = 2,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1344.12M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(new DateTime(2025,01,01)),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 112.01M,
                TipoSimulacao = 2
            };


            _simulacaoContext.Add(simulacaoSAC1);
            _simulacaoContext.Add(simulacaoPRICE1);
            _simulacaoContext.Add(simulacaoSAC2);
            _simulacaoContext.Add(simulacaoPRICE2);
            _simulacaoContext.SaveChanges();

            DateOnly dataReferencia = DateOnly.FromDateTime(DateTime.Now);

            // Act
            var result = await simulacaoRepository.ContaSimulacoesPorData(dataReferencia);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task ContaSimulacoesPorData_DeveRetornarExatamenteDois()
        {

            using var dbHackInMemory = SimulacaoRepositoryFixture.GetInMemoryContextDbHackContext();
            using var dbSimulacaoContextInMemory = SimulacaoRepositoryFixture.GetInMemoryContextSimulacaoContext();

            var _dbHack = dbHackInMemory;
            var _simulacaoContext = dbSimulacaoContextInMemory;

            SimulacaoRepository simulacaoRepository = new SimulacaoRepository(_dbHack, _simulacaoContext);

            Simulacao simulacaoSAC1 = new Simulacao()
            {
                IdSimulacao = 1,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1339.62M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 111.64M,
                TipoSimulacao = 1
            };

            Simulacao simulacaoPRICE1 = new Simulacao()
            {
                IdSimulacao = 1,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1344.12M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 112.01M,
                TipoSimulacao = 2
            };

            Simulacao simulacaoSAC2 = new Simulacao()
            {
                IdSimulacao = 2,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1339.62M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 111.64M,
                TipoSimulacao = 1
            };

            Simulacao simulacaoPRICE2 = new Simulacao()
            {
                IdSimulacao = 2,
                ValorDesejado = 1200,
                Prazo = 12,
                ValorTotalParcelas = 1344.12M,
                CodigoProduto = 1,
                DescricaoProduto = "Produto 1",
                DataReferencia = DateOnly.FromDateTime(DateTime.Now),
                TaxaJuros = 0.0179M,
                ValorMediaPrestacoes = 112.01M,
                TipoSimulacao = 2
            };


            _simulacaoContext.Add(simulacaoSAC1);
            _simulacaoContext.Add(simulacaoPRICE1);
            _simulacaoContext.Add(simulacaoSAC2);
            _simulacaoContext.Add(simulacaoPRICE2);
            _simulacaoContext.SaveChanges();

            DateOnly dataReferencia = DateOnly.FromDateTime(DateTime.Now);

            // Act
            var result = await simulacaoRepository.ContaSimulacoesPorData(dataReferencia);

            // Assert
            Assert.Equal(2, result);
        }

    }
}
