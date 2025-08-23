using System.Text.Json.Serialization;

namespace API_Simulacao_Hack.DTO
{
    public class RegistrosSimulacaoDTO
    {
        [JsonPropertyName("idSimulacao")]
        public int IdSimulacao { get; set; }
        [JsonPropertyName("valorDesejado")]
        public decimal ValorDesejado { get; set; }
        [JsonPropertyName("prazo")]
        public int Prazo { get; set; }
        [JsonPropertyName("valorTotalParcelas")]
        public decimal ValorTotalParcelas { get; set; }
    }
}
