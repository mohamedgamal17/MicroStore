using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace MicroStore.ShoppingGateway.ClinetSdk.Common
{
    [Serializable]
    public class Dimension
    {
        public double Length { get; set; } 

        public double Width { get; set; }

        public double Height { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DimensionUnit Unit { get; set; }
    }


    public enum DimensionUnit
    {
        CentiMeter,

        Inch 
    }
}
