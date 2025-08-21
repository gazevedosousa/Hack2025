using API_Simulacao_Hack.DTO;

namespace API_Simulacao_Hack.Interfaces.Services
{
    public interface ICalculoService
    {
        List<ResultadoSimulacaoDTO> CalculaParcelas(SolicitacaoSimulacaoDTO simulacaoDTO, decimal pcTaxaJuros);
        ResultadoSimulacaoDTO CalcularParcelasPrice(SolicitacaoSimulacaoDTO simulacaoDTO, decimal pcTaxaJuros);
        ResultadoSimulacaoDTO CalcularParcelasSAC(SolicitacaoSimulacaoDTO simulacaoDTO, decimal pcTaxaJuros);
    }
}
