using System.ComponentModel.DataAnnotations.Schema;

namespace API_Simulacao_Hack.Models;

[Table("Produto")]
public partial class Produto
{
    public int CoProduto { get; set; }

    public string NoProduto { get; set; } = null!;

    public decimal PcTaxaJuros { get; set; }

    public short NuMinimoMeses { get; set; }
    public short? NuMaximoMeses { get; set; }
    public decimal VrMinimo { get; set; }
    public decimal? VrMaximo { get; set; }
}
