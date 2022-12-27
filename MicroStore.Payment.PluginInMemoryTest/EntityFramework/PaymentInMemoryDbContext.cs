using Microsoft.EntityFrameworkCore;
using MicroStore.Payment.Application.Abstractions.Common;
using MicroStore.Payment.Domain;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
namespace MicroStore.Payment.PluginInMemoryTest.EntityFramework
{
    [ConnectionStringName("in_memory_db")]
    public class PaymentInMemoryDbContext : AbpDbContext<PaymentInMemoryDbContext> , IPaymentDbContext 
    {
        public DbSet<PaymentRequest> PaymentRequests { get; set; }
        public DbSet<PaymentSystem> PaymentSystems { get; set; }

        public DbSet<SettingsEntity> Settings { get; set; }

        public PaymentInMemoryDbContext(DbContextOptions<PaymentInMemoryDbContext> options) 
            : base(options)
        {

        }

    }
}
