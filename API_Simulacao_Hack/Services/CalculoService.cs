using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.Interfaces.Services;

namespace API_Simulacao_Hack.Services
{
    public class CalculoService: ICalculoService
    {
             public List<ResultadoSimulacaoDTO> CalculaParcelas(SolicitacaoSimulacaoDTO simulacaoDTO, decimal pcTaxaJuros)
        {
            List<ResultadoSimulacaoDTO> listaResultados = new List<ResultadoSimulacaoDTO>();

            ResultadoSimulacaoDTO resultadoPrice = CalcularParcelasPrice(simulacaoDTO, pcTaxaJuros);
            ResultadoSimulacaoDTO resultadoSAC = CalcularParcelasSAC(simulacaoDTO, pcTaxaJuros);

            listaResultados.Add(resultadoPrice);
            listaResultados.Add(resultadoSAC);

            return listaResultados;
        }
        // Método PRICE
        public ResultadoSimulacaoDTO CalcularParcelasPrice(SolicitacaoSimulacaoDTO simulacaoDTO, decimal pcTaxaJuros)
        {
            simulacaoDTO.Prazo = simulacaoDTO.Prazo == 0 ? 1 : simulacaoDTO.Prazo;

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
        public ResultadoSimulacaoDTO CalcularParcelasSAC(SolicitacaoSimulacaoDTO simulacaoDTO, decimal pcTaxaJuros)
        {
            simulacaoDTO.Prazo = simulacaoDTO.Prazo == 0 ? 1 : simulacaoDTO.Prazo;

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
