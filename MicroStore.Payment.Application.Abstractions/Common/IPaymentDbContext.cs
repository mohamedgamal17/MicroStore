using Microsoft.EntityFrameworkCore;
using MicroStore.Payment.Domain;
namespace MicroStore.Payment.Application.Abstractions.Common
{
    public interface IPaymentDbContext
    {
        DbSet<PaymentRequest> PaymentRequests { get; set; }

        DbSet<PaymentSystem> PaymentSystems { get; set; }
    }
}
