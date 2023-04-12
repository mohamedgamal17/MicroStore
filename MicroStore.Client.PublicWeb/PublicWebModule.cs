using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Bundling;
using MicroStore.Client.PublicWeb.Consts;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.Client.PublicWeb.Menus;
using MicroStore.Client.PublicWeb.Theming;
using Newtonsoft.Json.Converters;
using System.IdentityModel.Tokens.Jwt;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace MicroStore.Client.PublicWeb
{
    [DependsOn(typeof(MicroStoreAspNetCoreUIModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpBlobStoringModule),
        typeof(AbpBlobStoringMinioModule),
        typeof(AbpAspNetCoreMvcUiThemeSharedModule))]
    public class PublicWebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var host = context.Services.GetHostingEnvironment();

            var configuration = context.Services.GetConfiguration();

            ConfigureAuthentication(context.Services, configuration);

            Configure<AbpExceptionHandlingOptions>(options =>
            {
                options.SendExceptionsDetailsToClients = false;
                options.SendStackTraceToClients = false;
            });

            Configure<AbpAntiForgeryOptions>(opt =>
            {
                opt.AutoValidate = false;
            });

            ConfigureAutoMapper();


            ConfigureMinioStorage(configuration);

            context.Services.AddRazorPages()
             .AddRazorPagesOptions(opt =>
             {
                 opt.Conventions.AddPageRoute("/FrontEnd/Home", "");

             }).AddRazorRuntimeCompilation();

            context.Services.AddControllers().AddNewtonsoftJson(opt =>
            {
                var e = new StringEnumConverter();
                opt.SerializerSettings.Converters.Add(e);
            });

            context.Services.AddMvc();

            context.Services.AddControllers();

            context.Services.AddHttpContextAccessor();

            context.Services.AddMicroStoreClinet()
                .AddUserAccessTokenHandler();

            Configure<AbpNavigationOptions>(opt =>
            {
                opt.MainMenuNames.Add(ApplicationMenusDefaults.BackEnd);
                opt.MenuContributors.Add(new BackEndMenusContributor());
            });

            Configure<AbpThemingOptions>(opt =>
            {
                opt.DefaultThemeName = StandardApplicationTheme.Name;

                opt.Themes.Add<StandardApplicationTheme>();

            });

            Configure<AbpBundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Configure(StandardBundles.Styles.Global, bundle => { bundle.AddContributors(typeof(ApplicationThemeGlobalStyleContributor)); });

                options
                    .ScriptBundles
                    .Configure(StandardBundles.Scripts.Global, bundle => bundle.AddContributors(typeof(ApplicationThemeGlobalScriptContributor)));
            });
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            // Remove Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared Application Part

            Configure<IMvcBuilder>(opt =>
            {
                var appPart = opt.PartManager.ApplicationParts
                    .SingleOrDefault(x => ((AssemblyPart)x).Assembly == typeof(AbpAspNetCoreMvcUiThemeSharedModule).Assembly);

                if (appPart != null)
                {
                    opt.PartManager.ApplicationParts.Remove(appPart);
                }
            });
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            var config = context.GetConfiguration();

            app.UseAbpRequestLocalization();

            app.UseCorrelationId();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseAbpSerilogEnrichers();

            app.UseConfiguredEndpoints();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapControllers();

                endpoints.MapAreaControllerRoute(name: AreaNames.Administration,
                    areaName: AreaNames.Administration,
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });

        }




        private void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
           {
               options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
               options.Authority = configuration.GetValue<string>("IdentityProvider:Authority");
               options.ClientId = configuration.GetValue<string>("IdentityProvider:ClientId");
               options.ClientSecret = configuration.GetValue<string>("IdentityProvider:ClientSecret");
               options.GetClaimsFromUserInfoEndpoint = true;
               options.UsePkce = true;
               options.ResponseType = "code";
               options.Scope.Add("shoppinggateway.access");
               options.Scope.Add("mvcgateway.ordering.read");
               options.Scope.Add("mvcgateway.ordering.write");
               options.Scope.Add("mvcgateway.billing.read");
               options.Scope.Add("mvcgateway.billing.write");
               options.Scope.Add("mvcgateway.shipping.read");
               options.Scope.Add("mvcgateway.inventory.read");
               options.Scope.Add("mvcgateway.inventory.write");
               options.SaveTokens = true;

           });

            services.AddAccessTokenManagement();

        }


        private void ConfigureAutoMapper() => Configure<AbpAutoMapperOptions>(opt => opt.AddMaps<PublicWebModule>());


        private void ConfigureMinioStorage(IConfiguration config)
        {
            Configure<AbpBlobStoringOptions>(opt =>
            {
                opt.Containers.ConfigureDefault(container =>
                {
                    container.UseMinio(minio =>
                    {
                        minio.EndPoint = config.GetValue<string>("Minio:EndPoint");
                        minio.AccessKey = config.GetValue<string>("Minio:AccessKey");
                        minio.SecretKey = config.GetValue<string>("Minio:SecretKey");
                        minio.BucketName = config.GetValue<string>("Minio:BucketName");
                        minio.WithSSL = config.GetValue<bool>("Minio:WithSSL");
                        minio.CreateBucketIfNotExists = true;
                    });
                });
            });
        }
    }
}
