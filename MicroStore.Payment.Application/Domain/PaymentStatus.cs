namespace MicroStore.Payment.Application.Domain
{
    public enum PaymentStatus
    {

        Waiting = 0,

        Payed = 5,

        UnPayed = 10,

        Refunded = 15,

        Faild = 20
    }
}
