using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework;
using Volo.Abp.Modularity;
namespace MicroStore.IdentityProvider.Identity.Infrastructure
{
    [DependsOn(typeof(IdentityApplicationModule))]
    public class IdentityInfrastructureModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

            context.Services.AddDbContext<ApplicationIdentityDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"), sqlOpt =>
                {
                    sqlOpt.MigrationsAssembly(typeof(IdentityInfrastructureModule).Assembly.FullName);
                    sqlOpt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
                opt.EnableSensitiveDataLogging();
            },ServiceLifetime.Transient);


            context.Services.AddIdentity<ApplicationIdentityUser, ApplicationIdentityRole>(opt =>
            {
                opt.Stores.ProtectPersonalData = false;
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                opt.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                opt.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
                opt.ClaimsIdentity.EmailClaimType = JwtClaimTypes.Email;
            }).AddRoleManager<ApplicationRoleManager>()
            .AddUserManager<ApplicationUserManager>()
            .AddClaimsPrincipalFactory<ApplicationClaimPrincipalFactory>()
            .AddDefaultTokenProviders();

            context.Services.AddDataProtection();
        }

         
    }
}
