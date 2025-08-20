namespace API_Simulacao_Hack.Models;

public partial class Simulacao
{
    public int IdSimulacao { get; set; }
    public decimal ValorDesejado { get; set; }
    public int Prazo { get; set; }
    public decimal ValorTotalParcelas { get; set; }
    public int CodigoProduto { get; set; }
    public string DescricaoProduto { get; set; } = string.Empty;
    public DateOnly DataReferencia { get; set; }
    public decimal TaxaJuros { get; set; }

}
