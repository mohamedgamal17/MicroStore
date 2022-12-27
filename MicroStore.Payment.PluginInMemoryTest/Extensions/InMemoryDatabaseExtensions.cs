using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace MicroStore.Payment.PluginInMemoryTest.Extensions
{ 
    public static class InMemoryDatabaseExtensions
    {

        public static void UseInMemoryDb(this AbpDbContextOptions options , string? name = null, Action<InMemoryDbContextOptionsBuilder>? inMemoryOptionsAction = null)
        {
            options.Configure(ctx =>
            {
                ctx.UseInMemoryDatabase(name, inMemoryOptionsAction);
            });
        }

        public static DbContextOptionsBuilder UseInMemoryDatabase(
          this AbpDbContextConfigurationContext context,
          string? name = null,       
           Action<InMemoryDbContextOptionsBuilder>? inMemoryOptionsAction = null)
        {

            return context.DbContextOptions.UseInMemoryDatabase(name ?? Guid.NewGuid().ToString(), inMemoryOptionsAction);
        }
    }

    public static  class AbpDbContextConfigurationContextSqlServerExtensions
    {
      
    }
}
