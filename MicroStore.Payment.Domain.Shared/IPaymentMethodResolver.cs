namespace MicroStore.Payment.Domain.Shared
{
    public interface IPaymentMethodResolver
    {
        IPaymentMethod Resolve(string paymentGatewayName);
    }
}
