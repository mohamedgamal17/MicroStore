using Microsoft.EntityFrameworkCore;
using MicroStore.Payment.Domain.Shared.Domain;

namespace MicroStore.Payment.Application.Common
{
    public interface IPaymentDbContext
    {
        DbSet<PaymentRequest> PaymentRequests { get; set; }
    }
}
