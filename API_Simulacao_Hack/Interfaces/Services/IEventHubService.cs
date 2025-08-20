using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.DTO.EventHub;

namespace API_Simulacao_Hack.Interfaces.Services
{
    public interface IEventHubService
    {
        Task EnviaEvento(EventHubDTO eventHubDTO);
        EventHubDTO CriarMensagemParaEventHub(SolicitacaoSimulacaoDTO simulacao, RetornoSimulacaoDTO retornoSimulacao);
    }
}
