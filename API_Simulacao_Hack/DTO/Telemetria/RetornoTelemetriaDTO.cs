using System.Text.Json.Serialization;

namespace API_Simulacao_Hack.DTO
{
    public class RetornoTelemetriaDTO
    {
        [JsonPropertyName("dataReferencia")]
        public DateOnly DataReferencia { get; set; }

        [JsonPropertyName("listaEndpoints")]
        public List<TelemetriaDTO> ListaEndpoints { get; set; } = new List<TelemetriaDTO>();
    }
}
