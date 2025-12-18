using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace MicroStore.BuildingBlocks.AspNetCore.Infrastructure
{
    public class SnakeCaseParamsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();
            else
            {
                foreach (var item in operation.Parameters)
                {
                    if (!item.Required)
                    {
                        item.Name = item.Name.ToSnakeCase();
                    }                   
                }
            }
        }
    }
}
