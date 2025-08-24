namespace API_Simulacao_Hack.Models;

public partial class Simulacao
{
    public int Id { get; set; }
    public int IdSimulacao { get; set; }
    public decimal ValorDesejado { get; set; }
    public int Prazo { get; set; }
    public decimal ValorTotalParcelas { get; set; }
    public int CodigoProduto { get; set; }
    public string DescricaoProduto { get; set; } = string.Empty;
    public DateOnly DataReferencia { get; set; }
    public decimal TaxaJuros { get; set; }
    public decimal ValorMediaPrestacoes { get; set; }
    public string TipoSimulacao { get; set; } = string.Empty;
}
