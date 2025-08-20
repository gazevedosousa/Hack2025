using System.Text.Json.Serialization;

namespace API_Simulacao_Hack.DTO
{
    public class TelemetriaDTO
    {
        [JsonPropertyName("nomeApi")]
        public string NomeApi { get; set; }
        
        [JsonPropertyName("qtdRequisicoes")]
        public int QtdRequisicoes { get; set; }

        [JsonPropertyName("tempoMedio")]
        public double TempoMedio { get; set; }

        [JsonPropertyName("tempoMinimo")]
        public double TempoMinimo { get; set; }

        [JsonPropertyName("tempoMaximo")]
        public double TempoMaximo { get; set; }

        [JsonPropertyName("percentualSucesso")]
        public double PercentualSucesso { get; set; }
    }
}
