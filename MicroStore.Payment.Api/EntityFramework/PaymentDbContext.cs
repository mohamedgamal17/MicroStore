using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Volo.Abp.EntityFrameworkCore;

namespace MicroStore.Payment.Api.EntityFramework
{
    public class PaymentDbContext : AbpDbContext<PaymentDbContext>
    {
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
