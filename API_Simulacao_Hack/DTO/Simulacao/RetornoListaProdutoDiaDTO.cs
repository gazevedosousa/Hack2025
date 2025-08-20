using System.Text.Json.Serialization;

namespace API_Simulacao_Hack.DTO
{
    public class RetornoListaProdutoDiaDTO
    {
        [JsonPropertyName("dataReferencia")]
        public DateOnly DataReferencia { get; set; }

        [JsonPropertyName("simulacoes")]
        public List<RetornoSimulacoesProdutoDiaDTO> Simulacoes { get; set; } = new List<RetornoSimulacoesProdutoDiaDTO>();
    }
}
