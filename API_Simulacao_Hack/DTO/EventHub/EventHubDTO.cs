namespace API_Simulacao_Hack.DTO.EventHub
{
    public class EventHubDTO
    {
        public SolicitacaoSimulacaoDTO SolicitacaoSimulacao { get; set; } = new SolicitacaoSimulacaoDTO();
        public RetornoSimulacaoDTO RetornoSimulacao { get; set; } = new RetornoSimulacaoDTO();
    }
}
