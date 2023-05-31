#nullable disable
using Microsoft.EntityFrameworkCore;
using MicroStore.Payment.Application.Common;
using MicroStore.Payment.Domain;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
namespace MicroStore.Payment.Application.EntityFramework
{
    [ExposeServices(typeof(DbContext), IncludeDefaults = true, IncludeSelf = true)]
    [ConnectionStringName("DefaultConnection")]
    public class PaymentDbContext : AbpDbContext<PaymentDbContext> , IPaymentDbContext , ITransientDependency
    {
        public DbSet<PaymentRequest> PaymentRequests { get; set; }
        public DbSet<PaymentSystem> PaymentSystems { get ; set ; }

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
