
using System.Text.Json;

namespace MicroStore.BuildingBlocks.AspNetCore.Infrastructure
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public static SnakeCaseNamingPolicy Instance { get; } = new SnakeCaseNamingPolicy();
        public override string ConvertName(string name)
        {
            return name.ToSnakeCase();
        }
    }
}
