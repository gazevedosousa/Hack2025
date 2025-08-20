using System.Text.Json.Serialization;

namespace API_Simulacao_Hack.DTO
{
    public class RetornoSimulacaoDTO
    {
        [JsonPropertyName("idSimulacao")]
        public int IdSimulacao { get; set; }
        [JsonPropertyName("codigoProduto")]
        public int CodigoProduto { get; set; }
        [JsonPropertyName("descricaoProduto")]
        public string DescricaoProduto { get; set; } = string.Empty;
        [JsonPropertyName("taxaJuros")]
        public decimal TaxaJuros { get; set; }
        [JsonPropertyName("resultadoSimulacao")]
        public List<ResultadoSimulacaoDTO> ResultadosSimulacao { get; set; } = new List<ResultadoSimulacaoDTO>();
        
    }
}
