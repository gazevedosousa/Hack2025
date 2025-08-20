using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.Util;
using System;
using System.Linq;
using Xunit;

namespace API_Simulacao_Hack.Test
{
    public class CalculoUtilTest
    {
        [Fact]
        public void CalculaParcelas_DeveRetornarDoisResultados()
        {
            // Arrange
            var dto = new SolicitacaoSimulacaoDTO { ValorDesejado = 1000m, Prazo = 5 };
            decimal taxaJuros = 0.01m;

            // Act
            var resultados = CalculoUtil.CalculaParcelas(dto, taxaJuros);

            // Assert
            Assert.Equal(2, resultados.Count);
            Assert.Contains(resultados, r => r.Tipo == "PRICE");
            Assert.Contains(resultados, r => r.Tipo == "SAC");
        }

        [Fact]
        public void Price_Parcelas_SomaPrestacoesIgualValorDesejadoMaisJuros()
        {
            // Arrange
            var dto = new SolicitacaoSimulacaoDTO { ValorDesejado = 1000m, Prazo = 3 };
            decimal taxaJuros = 0.1m;

            // Act
            var resultado = CalculoUtil.CalculaParcelas(dto, taxaJuros)
                                        .First(r => r.Tipo == "PRICE");

            decimal somaPrestacoes = resultado.Parcelas.Sum(p => p.ValorPrestacao);

            // Assert: soma das prestações >= valor desejado (inclui juros)
            Assert.True(somaPrestacoes > dto.ValorDesejado);
            Assert.Equal(dto.Prazo, resultado.Parcelas.Count);
        }

        [Fact]
        public void SAC_Parcelas_DeveDiminuirComPrazo()
        {
            // Arrange
            var dto = new SolicitacaoSimulacaoDTO { ValorDesejado = 1200m, Prazo = 4 };
            decimal taxaJuros = 0.05m;

            // Act
            var resultado = CalculoUtil.CalculaParcelas(dto, taxaJuros)
                                        .First(r => r.Tipo == "SAC");

            var valoresPrestacao = resultado.Parcelas.Select(p => p.ValorPrestacao).ToList();

            // Assert: cada prestação deve ser menor que a anterior
            for (int i = 1; i < valoresPrestacao.Count; i++)
            {
                Assert.True(valoresPrestacao[i] <= valoresPrestacao[i - 1]);
            }
        }

        [Fact]
        public void CalculaParcelas_PrazoUm_DeveRetornarUmaParcelaComValorTotal()
        {
            // Arrange
            var dto = new SolicitacaoSimulacaoDTO { ValorDesejado = 500m, Prazo = 1 };
            decimal taxaJuros = 0.1m;

            // Act
            var resultados = CalculoUtil.CalculaParcelas(dto, taxaJuros);

            foreach (var resultado in resultados)
            {
                Assert.Single(resultado.Parcelas);
                var parcela = resultado.Parcelas[0];
                Assert.Equal(1, parcela.Numero);
                Assert.True(parcela.ValorPrestacao > 0);
            }
        }

        [Fact]
        public void CalculaParcelas_ValoresNegativos_DeveRetornarParcelasNegativas()
        {
            // Arrange
            var dto = new SolicitacaoSimulacaoDTO { ValorDesejado = -1000m, Prazo = 3 };
            decimal taxaJuros = 0.05m;

            // Act
            var resultados = CalculoUtil.CalculaParcelas(dto, taxaJuros);

            foreach (var resultado in resultados)
            {
                foreach (var parcela in resultado.Parcelas)
                {
                    Assert.True(parcela.ValorPrestacao < 0);
                    Assert.True(parcela.ValorAmortizacao < 0);
                }
            }
        }

        [Fact]
        public void CalculaParcelas_ValoresDecimaisPrecisaoDuasCasas()
        {
            // Arrange
            var dto = new SolicitacaoSimulacaoDTO { ValorDesejado = 1234.56m, Prazo = 3 };
            decimal taxaJuros = 0.07m;

            // Act
            var resultados = CalculoUtil.CalculaParcelas(dto, taxaJuros);

            foreach (var resultado in resultados)
            {
                foreach (var parcela in resultado.Parcelas)
                {
                    Assert.Equal(2, BitConverter.GetBytes(decimal.GetBits(parcela.ValorPrestacao)[3])[2]); // arredondamento correto
                }
            }
        }

        [Fact]
        public void CalculaParcelas_PRICE_ConferenciaCalculo()
        {
            // Arrange
            var simulacao = new SolicitacaoSimulacaoDTO
            {
                ValorDesejado = 1000m,
                Prazo = 3
            };
            decimal taxaJuros = 0.1m; // 10%

            // Act
            var resultado = CalculoUtil.CalculaParcelas(simulacao, taxaJuros)
                                        .First(r => r.Tipo == "PRICE");

            decimal saldoDevedor = simulacao.ValorDesejado;
            foreach (var parcela in resultado.Parcelas)
            {
                // Conferir que juros = saldoDevedor * taxa (com tolerância 0,01)
                var jurosEsperado = Math.Round(saldoDevedor * taxaJuros, 2);
                Assert.InRange(parcela.ValorJuros, jurosEsperado - 0.01m, jurosEsperado + 0.01m);

                // Conferir que valorPrestacao = amortizacao + juros (com tolerância 0,01)
                var amortizacaoEsperada = Math.Round(parcela.ValorPrestacao - parcela.ValorJuros, 2);
                Assert.InRange(parcela.ValorAmortizacao, amortizacaoEsperada - 0.01m, amortizacaoEsperada + 0.01m);

                // Atualiza saldo devedor
                saldoDevedor -= parcela.ValorAmortizacao;
            }

            // Saldo final deve ser aproximadamente zero (tolerância 0,01)
            Assert.InRange(saldoDevedor, -0.01m, 0.01m);
        }

        [Fact]
        public void CalculaParcelas_SAC_ConferenciaCalculo()
        {
            // Arrange
            var simulacao = new SolicitacaoSimulacaoDTO
            {
                ValorDesejado = 1200m,
                Prazo = 4
            };
            decimal taxaJuros = 0.05m; // 5%

            // Act
            var resultado = CalculoUtil.CalculaParcelas(simulacao, taxaJuros)
                                        .First(r => r.Tipo == "SAC");

            decimal saldoDevedor = simulacao.ValorDesejado;
            decimal amortizacaoFixa = Math.Round(simulacao.ValorDesejado / simulacao.Prazo, 2);

            foreach (var parcela in resultado.Parcelas)
            {
                // Juros = saldoDevedor * taxa
                var jurosEsperado = Math.Round(saldoDevedor * taxaJuros, 2);
                Assert.Equal(jurosEsperado, parcela.ValorJuros);

                // Prestacao = amortizacao + juros
                var prestacaoEsperada = Math.Round(amortizacaoFixa + jurosEsperado, 2);
                Assert.Equal(prestacaoEsperada, parcela.ValorPrestacao);

                // Amortizacao fixa
                Assert.Equal(amortizacaoFixa, parcela.ValorAmortizacao);

                // Atualiza saldo devedor
                saldoDevedor -= parcela.ValorAmortizacao;
            }

            // Saldo final deve ser aproximadamente zero
            Assert.True(Math.Abs(saldoDevedor) < 0.01m);
        }

        [Fact]
        public void CalculaParcelas_PRICE_E_SAC_SaldoDevedorFinal_Zerado()
        {
            // Arrange
            var simulacao = new SolicitacaoSimulacaoDTO
            {
                ValorDesejado = 5000m,
                Prazo = 5
            };
            decimal taxaJuros = 0.08m;

            // Act
            var resultados = CalculoUtil.CalculaParcelas(simulacao, taxaJuros);

            foreach (var resultado in resultados)
            {
                decimal saldoDevedor = simulacao.ValorDesejado;
                foreach (var parcela in resultado.Parcelas)
                {
                    saldoDevedor -= parcela.ValorAmortizacao;
                }
                // Saldo final deve ser aproximadamente zero
                Assert.InRange(saldoDevedor, -0.01m, 0.01m);
            }
        }

        [Fact]
        public void CalculaParcelas_SAC_ParcelasDecrescentes()
        {
            // Arrange
            var simulacao = new SolicitacaoSimulacaoDTO
            {
                ValorDesejado = 1000m,
                Prazo = 4
            };
            decimal taxaJuros = 0.1m;

            // Act
            var resultadoSAC = CalculoUtil.CalculaParcelas(simulacao, taxaJuros)
                                          .First(r => r.Tipo == "SAC");

            var prestacoes = resultadoSAC.Parcelas.Select(p => p.ValorPrestacao).ToList();

            // Assert: cada prestação deve ser menor ou igual à anterior
            for (int i = 1; i < prestacoes.Count; i++)
            {
                Assert.True(prestacoes[i] <= prestacoes[i - 1]);
            }
        }
    }
}
