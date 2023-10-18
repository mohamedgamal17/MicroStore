using Microsoft.EntityFrameworkCore;
using MicroStore.Payment.Application.Domain;
using MicroStore.Payment.Domain.Shared.Configuration;

namespace MicroStore.Payment.Application.Common
{
    public interface IPaymentDbContext
    {
        DbSet<PaymentRequest> PaymentRequests { get; set; }

    }
}
