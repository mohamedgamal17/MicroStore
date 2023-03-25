using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Infrastructure;
using MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework;
using MicroStore.TestBase.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Respawn;
using Respawn.Graph;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
namespace MicroStore.IdentityProvider.Identity.Application.Tests
{
    [DependsOn(typeof(IdentityInfrastructureModule),
        typeof(IdentityApplicationModule),
        typeof(AbpAutofacModule))]
    public class IdentityApplicationTestModule : AbpModule
    {
        private readonly JsonSerializerSettings _jsonSerilizerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DomainModelContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
        };


        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddIdentity<ApplicationIdentityUser, ApplicationIdentityRole>(opt =>
            {
                opt.Stores.ProtectPersonalData = false;
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            }).AddUserManager<ApplicationUserManager>()
               .AddRoleManager<ApplicationRoleManager>()
               .AddDefaultTokenProviders();

        }
        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            var config = context.ServiceProvider.GetRequiredService<IConfiguration>();

            var respawner = Respawner.CreateAsync(config.GetConnectionString("DefaultConnection")!, new RespawnerOptions
            {
                TablesToIgnore = new Table[]
                {
                    "__EFMigrationsHistory"
                }
            }).Result;

            respawner.ResetAsync(config.GetConnectionString("DefaultConnection")!).Wait();
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

                db.Database.Migrate();


                SeedData(scope.ServiceProvider);
            }
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            var config = context.ServiceProvider.GetRequiredService<IConfiguration>();

            var respawner = Respawner.CreateAsync(config.GetConnectionString("DefaultConnection"), new RespawnerOptions
            {
                TablesToIgnore = new Table[]
                {
                    "__EFMigrationsHistory"
                }
            }).Result;


            respawner.ResetAsync(config.GetConnectionString("DefaultConnection")).Wait();
        }


        private void SeedData(IServiceProvider serviceProvider)
        {
            using (var stream = new StreamReader(@"Dummies\Data.json"))
            {

                var userManager = serviceProvider.GetRequiredService<ApplicationUserManager>();
                var roleManager = serviceProvider.GetRequiredService<ApplicationRoleManager>();

                var json = stream.ReadToEnd();

                var data = JsonConvert.DeserializeObject<JsonWrapper<UserReadModel>>(json, _jsonSerilizerSettings);

                if (data != null)
                {
                    foreach (var user in data.Data)
                    {


                        CreateRoles(roleManager, user.UserRoles);


                        var identityUser = new ApplicationIdentityUser
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            UserName = user.UserName,
                            UserClaims = user.UserClaims.Select(x => new ApplicationIdentityUserClaim { ClaimType = x.Type, ClaimValue = x.Value }).ToList()
                        };

                        var id = userManager.CreateAsync(identityUser, user.Password).Result;


                        if (!id.Succeeded)
                        {
                            throw new InvalidOperationException(id.Errors.Select(x => x.Description).JoinAsString("\n"));
                        }

                        userManager.AddToRolesAsync(identityUser, user.UserRoles.Select(x => x.Name)).Wait();
                    }
                }
            }
        }

        private void CreateRoles(ApplicationRoleManager rolemanager, List<RoleReadModel> userRoles)
        {
            foreach (var role in userRoles)
            {
                rolemanager.CreateAsync(new ApplicationIdentityRole { Name = role.Name, Description = role.Description }).Wait();
            }
        }
    }
}
