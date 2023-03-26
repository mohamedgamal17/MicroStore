using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using MicroStore.Client.PublicWeb.Consts;
using MicroStore.Client.PublicWeb.Infrastructure;
using Newtonsoft.Json.Converters;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace MicroStore.Client.PublicWeb
{
    [DependsOn(typeof(AbpAutoMapperModule),
        typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAspNetCoreMvcModule))]
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

            context.Services.AddTransient<IWorkContext, DefaultWorkContext>();

            context.Services.AddHttpContextAccessor();

            context.Services.AddMicroStoreClinet()
                .AddUserAccessTokenHandler();
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
    }
}
