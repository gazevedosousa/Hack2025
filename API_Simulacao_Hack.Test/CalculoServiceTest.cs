using API_Simulacao_Hack.Services;
using API_Simulacao_Hack.DTO;

namespace API_Simulacao_Hack.Test
{
    public class CalculoServiceTest
    {
        private readonly CalculoService _service = new CalculoService();

        [Fact]
        public void CalculaParcelas_DeveRetornarDoisResultados_QuandoParametrosValidos()
        {
            var dto = new SolicitacaoSimulacaoDTO { ValorDesejado = 1000m, Prazo = 12 };
            var resultados = _service.CalculaParcelas(dto, 0.01m);

            Assert.Equal(2, resultados.Count);
            Assert.Contains(resultados, r => r.Tipo == "PRICE");
            Assert.Contains(resultados, r => r.Tipo == "SAC");
        }

        [Fact]
        public void CalcularParcelasPrice_DeveRetornarParcelasCorretas()
        {
            var dto = new SolicitacaoSimulacaoDTO { ValorDesejado = 1200m, Prazo = 12 };
            var resultado = _service.CalcularParcelasPrice(dto, 0.02m);

            Assert.Equal("PRICE", resultado.Tipo);
            Assert.Equal(12, resultado.Parcelas.Count);
            Assert.All(resultado.Parcelas, p => Assert.True(p.ValorPrestacao > 0));
        }

        [Fact]
        public void CalcularParcelasSAC_DeveRetornarParcelasCorretas()
        {
            var dto = new SolicitacaoSimulacaoDTO { ValorDesejado = 1200m, Prazo = 12 };
            var resultado = _service.CalcularParcelasSAC(dto, 0.02m);

            Assert.Equal("SAC", resultado.Tipo);
            Assert.Equal(12, resultado.Parcelas.Count);
            Assert.All(resultado.Parcelas, p => Assert.True(p.ValorPrestacao > 0));
        }

        [Fact]
        public void CalculaParcelas_DeveRetornarParcelasComValorZero_QuandoValorDesejadoZero()
        {
            var dto = new SolicitacaoSimulacaoDTO { ValorDesejado = 0m, Prazo = 12 };
            var resultados = _service.CalculaParcelas(dto, 0.01m);

            Assert.All(resultados, r => Assert.All(r.Parcelas, p => Assert.Equal(0, p.ValorPrestacao)));
        }
    }

}
