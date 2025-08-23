using System.Text.Json.Serialization;

namespace API_Simulacao_Hack.DTO
{
    public class RetornoSimulacoesProdutoDiaDTO
    {
        [JsonPropertyName("codigoProduto")]
        public int CodigoProduto { get; set; }

        [JsonPropertyName("descricaoProduto")]
        public string DescricaoProduto { get; set; } = string.Empty;

        [JsonPropertyName("taxaMediaJuro")]
        public decimal TaxaMediaJuro { get; set; }

        [JsonPropertyName("valorMedioPrestacao")]
        public decimal ValorMedioPrestacao { get; set; }

        [JsonPropertyName("valorTotalDesejado")]
        public decimal ValorTotalDesejado { get; set; }

        [JsonPropertyName("valorTotalCredito")]
        public decimal ValorTotalCredito { get; set; }
    }
}
