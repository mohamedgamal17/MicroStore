using MicroStore.Payment.Domain.Shared.Dtos;

namespace MicroStore.Payment.Domain.Shared
{
    public interface IPaymentSystemManager
    {
        Task TryToCreate(string name, string displayName, string image);
        Task TryToDeleate(string name , string displayName, string image);
        Task<PaymentSystemDto> GetPaymentSystem(string name);
    }
}
