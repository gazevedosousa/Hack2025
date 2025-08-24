
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API_Simulacao_Hack.Enum
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                var enumDescriptions = TipoSimulacaoEnum.GetValues(context.Type)
                    .Cast<TipoSimulacaoEnum>()
                    .Select(e => $"{Convert.ToInt32(e)} = {e}")
                    .ToList();

                schema.Description += string.Join(", ", enumDescriptions);
            }
        }
    }
}
