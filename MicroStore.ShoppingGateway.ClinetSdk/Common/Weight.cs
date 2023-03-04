using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace MicroStore.ShoppingGateway.ClinetSdk.Common
{
    [Serializable]
    public class Weight
    {
        public double  Value { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public WeightUnit Unit { get; set; }
    }

    public enum WeightUnit
    {
        [EnumMember(Value = "g")]
        Gram = 0,

        [EnumMember (Value = "kg")]
        KiloGram = 1,

        [EnumMember(Value ="lb")]
        Pound = 2
    }
}
