using System.Text.Json.Serialization;

namespace API_Simulacao_Hack.DTO
{
    public class SolicitacaoSimulacaoDTO
    {
        [JsonPropertyName("valorDesejado")]
        public decimal ValorDesejado { get; set; }

        [JsonPropertyName("prazo")]
        public int Prazo { get; set; }
    }
}
