using IdentityModel;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.IdentityProvider.Identity.Application;
using MicroStore.IdentityProvider.Identity.Infrastructure;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Modularity;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.VirtualFileSystem;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using Volo.Abp.AutoMapper;
using MicroStore.AspNetCore.UI;

namespace MicroStore.IdentityProvider.Identity.Web
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiThemeSharedModule),
              typeof(IdentityInfrastructureModule),
        typeof(MicroStoreAspNetCoreUIModule),
         typeof(MicroStoreAspNetCoreModule))]
    public class IdentityWebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt => opt.AddMaps<IdentityWebModule>());

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

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<IdentityWebModule>("MicroStore.IdentityProvider.Identity.Web");
            });

        }



    }
}