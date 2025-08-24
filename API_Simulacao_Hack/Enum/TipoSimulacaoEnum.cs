using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace API_Simulacao_Hack.Enum
{
    public enum TipoSimulacaoEnum
    {
        [Description("SAC")]
        SAC = 1,
        [Description("PRICE")]
        PRICE = 2
    }
}
