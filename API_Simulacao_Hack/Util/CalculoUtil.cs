using API_Simulacao_Hack.DTO;

namespace API_Simulacao_Hack.Util
{
    public static class CalculoUtil
    {

        public static List<ResultadoSimulacaoDTO> CalculaParcelas(SolicitacaoSimulacaoDTO simulacaoDTO, decimal pcTaxaJuros)
        {
            List<ResultadoSimulacaoDTO> listaResultados = new List<ResultadoSimulacaoDTO>();

            ResultadoSimulacaoDTO resultadoPrice = CalcularParcelasPrice(simulacaoDTO, pcTaxaJuros);
            ResultadoSimulacaoDTO resultadoSAC = CalcularParcelasSAC(simulacaoDTO, pcTaxaJuros);

            listaResultados.Add(resultadoPrice);
            listaResultados.Add(resultadoSAC);

            return listaResultados;
        }
        // Método PRICE
        private static ResultadoSimulacaoDTO CalcularParcelasPrice(SolicitacaoSimulacaoDTO simulacaoDTO, decimal pcTaxaJuros)
        {
            var parcelas = new List<ParcelasDTO>();
            var fator = (decimal)Math.Pow((double)(1 + pcTaxaJuros), simulacaoDTO.Prazo);
            var valorPrestacao = simulacaoDTO.ValorDesejado * (pcTaxaJuros * fator) / (fator - 1);
            decimal saldoDevedor = simulacaoDTO.ValorDesejado;

            for (int i = 1; i <= simulacaoDTO.Prazo; i++)
            {
                var valorJuros = saldoDevedor * pcTaxaJuros;
                var valorAmortizacao = valorPrestacao - valorJuros;
                parcelas.Add(new ParcelasDTO
                {
                    Numero = i,
                    ValorAmortizacao = Math.Round(valorAmortizacao, 2),
                    ValorJuros = Math.Round(valorJuros, 2),
                    ValorPrestacao = Math.Round(valorPrestacao, 2)
                });
                saldoDevedor -= valorAmortizacao;
            }

            return new ResultadoSimulacaoDTO()
            {
                Tipo = "PRICE",
                Parcelas = parcelas
            };
        }

        // Método SAC
        private static ResultadoSimulacaoDTO CalcularParcelasSAC(SolicitacaoSimulacaoDTO simulacaoDTO, decimal pcTaxaJuros)
        {
            var parcelas = new List<ParcelasDTO>();
            var valorAmortizacao = simulacaoDTO.ValorDesejado / simulacaoDTO.Prazo;
            decimal saldoDevedor = simulacaoDTO.ValorDesejado;

            for (int i = 1; i <= simulacaoDTO.Prazo; i++)
            {
                var valorJuros = saldoDevedor * pcTaxaJuros;
                var valorPrestacao = valorAmortizacao + valorJuros;
                parcelas.Add(new ParcelasDTO
                {
                    Numero = i,
                    ValorAmortizacao = Math.Round(valorAmortizacao, 2),
                    ValorJuros = Math.Round(valorJuros, 2),
                    ValorPrestacao = Math.Round(valorPrestacao, 2)
                });
                saldoDevedor -= valorAmortizacao;
            }

            return new ResultadoSimulacaoDTO()
            {
                Tipo = "SAC",
                Parcelas = parcelas
            };
        }
    }

}
