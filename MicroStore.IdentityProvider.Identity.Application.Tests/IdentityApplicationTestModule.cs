using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.IdentityProvider.Identity.Infrastructure;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
namespace MicroStore.IdentityProvider.Identity.Application.Tests
{
    [DependsOn(typeof(IdentityInfrastructureModule),
        typeof(IdentityApplicationModule),
        typeof(MediatorModule),
        typeof(AbpAutofacModule))]
    public class IdentityApplicationTestModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

                db.Database.Migrate();
            }



        }
    }
}
