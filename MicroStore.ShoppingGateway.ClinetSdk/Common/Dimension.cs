using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace MicroStore.ShoppingGateway.ClinetSdk.Common
{
    [Serializable]
    public class Dimension
    {
        public double Lenght { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DimensionUnit Unit { get; set; }
    }


    public enum DimensionUnit
    {
        [EnumMember(Value = "none")]
        None ,

        [EnumMember(Value = "cm")]
        Centimeters,

        [EnumMember(Value = "inch")]
        Inch,


    }
}
