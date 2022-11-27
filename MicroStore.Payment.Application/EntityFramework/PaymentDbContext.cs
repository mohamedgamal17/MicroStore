#nullable disable
using Microsoft.EntityFrameworkCore;
using MicroStore.Payment.Application.Common;
using MicroStore.Payment.Domain.Shared.Domain;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
namespace MicroStore.Payment.Application.EntityFramework
{

    [ConnectionStringName("DefaultConnection")]
    public class PaymentDbContext : AbpDbContext<PaymentDbContext> , IPaymentDbContext
    {
        public DbSet<PaymentRequest> PaymentRequests { get; set; }

        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
