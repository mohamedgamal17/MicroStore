using System.Runtime.Serialization;

namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing
{
    public enum PaymentStatus
    {
        [EnumMember(Value ="Waiting")]
        Waiting ,

        [EnumMember(Value ="Payed")]
        Payed ,

        [EnumMember(Value = "UnPayed")]
        UnPayed ,


        [EnumMember(Value = "Refunded")]
        Refunded ,

        [EnumMember(Value = "Faild")]
        Faild
    }
}
