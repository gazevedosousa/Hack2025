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
    private readonly ILogger<EventHubService> _logger;
    private readonly EventHubProducerClient _ehProducer;

    public EventHubService(
        ILogger<EventHubService> logger,
        EventHubProducerClient ehProducer)
    {
        _logger = logger;
        _ehProducer = ehProducer;
    }

    public async Task EnviaEvento(EventHubDTO eventHubDTO)
    {
        try
        {
            var eventBatch = await _ehProducer.CreateBatchAsync();

            var msg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(eventHubDTO));

            if (!eventBatch.TryAdd(new EventData(msg)))
            {
                await _ehProducer.SendAsync(eventBatch);
                eventBatch.Dispose();
                eventBatch = await _ehProducer.CreateBatchAsync();

                if (!eventBatch.TryAdd(new EventData(msg)))
                {
                    throw new CustomException("Erro no batch para envio das mensagens");
                }
            }

            if (eventBatch.SizeInBytes > 0)
            {
                await _ehProducer.SendAsync(eventBatch);
            }

            eventBatch.Dispose();
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
