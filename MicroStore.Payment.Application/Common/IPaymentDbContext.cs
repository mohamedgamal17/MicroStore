using Microsoft.EntityFrameworkCore;
using MicroStore.Payment.Application.Domain;
namespace MicroStore.Payment.Application.Common
{
    public interface IPaymentDbContext
    {
        DbSet<PaymentRequest> PaymentRequests { get; set; }

        DbSet<PaymentSystem> PaymentSystems { get; set; }
    }
}
