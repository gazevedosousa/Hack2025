using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.DTO.EventHub;
using API_Simulacao_Hack.Interfaces.Services;
using API_Simulacao_Hack.Util.Base;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace API_Simulacao_Hack.Services;

[ExcludeFromCodeCoverage]
public class EventHubService: IEventHubService
{
    private readonly EventHubProducerClient _ehProducer;

    public EventHubService(
        EventHubProducerClient ehProducer)
    {
        _ehProducer = ehProducer;
    }

    public async Task EnviaEvento(EventHubDTO eventHubDTO)
    {
        try
        {
            var batch = await _ehProducer.CreateBatchAsync();

            var msgEventHub = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(eventHubDTO));

            if (!batch.TryAdd(new EventData(msgEventHub)))
            {
                await _ehProducer.SendAsync(batch);
                batch.Dispose();
                batch = await _ehProducer.CreateBatchAsync();

                if (!batch.TryAdd(new EventData(msgEventHub)))
                {
                    throw new CustomException("Erro no batch para envio das mensagens");
                }
            }

            if (batch.SizeInBytes > 0)
            {
                await _ehProducer.SendAsync(batch);
            }

            batch.Dispose();
        }
        catch
        {
            throw new CustomException("Erro ao enviar mensagem ao EventHub");
        }
    }

    public EventHubDTO CriarMensagemParaEventHub(SolicitacaoSimulacaoDTO simulacao, RetornoSimulacaoDTO retornoSimulacao)
    {
        return new EventHubDTO()
        {
            SolicitacaoSimulacao = simulacao,
            RetornoSimulacao = retornoSimulacao
        };
    }

}
