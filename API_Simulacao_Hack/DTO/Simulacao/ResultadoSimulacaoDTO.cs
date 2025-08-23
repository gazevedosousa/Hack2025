using System.Text.Json.Serialization;

namespace API_Simulacao_Hack.DTO
{
    public class ResultadoSimulacaoDTO
    {
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } = string.Empty;

        [JsonPropertyName("parcelas")]
        public List<ParcelasDTO> Parcelas { get; set; } = new List<ParcelasDTO>();
    }

    public class ParcelasDTO
    {
        [JsonPropertyName("numero")]
        public int Numero { get; set; }
        [JsonPropertyName("valorAmortizacao")]
        public decimal ValorAmortizacao { get; set; }
        [JsonPropertyName("valorJuros")]
        public decimal ValorJuros { get; set; }
        [JsonPropertyName("valorPrestacao")]
        public decimal ValorPrestacao { get; set; }
    }
}
