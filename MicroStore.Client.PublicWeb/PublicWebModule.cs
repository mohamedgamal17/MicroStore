﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Bundling;
using MicroStore.Client.PublicWeb.Configuration;
using MicroStore.Client.PublicWeb.Consts;
using MicroStore.Client.PublicWeb.Menus;
using MicroStore.Client.PublicWeb.Security;
using MicroStore.Client.PublicWeb.Theming;
using MicroStore.ShoppingGateway.ClinetSdk.Extensions;
using Newtonsoft.Json.Converters;
using System.Security.Claims;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using FluentValidation.AspNetCore;
using Volo.Abp.FluentValidation;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.Client.PublicWeb.Infrastructure;
namespace MicroStore.Client.PublicWeb
{
    [DependsOn(typeof(MicroStoreAspNetCoreUIModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(AbpFluentValidationModule))]
    public class PublicWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

            var appsettings = config.Get<ApplicationSettings>();

            context.Services.AddSingleton(appsettings);
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var host = context.Services.GetHostingEnvironment();

            var configuration = context.Services.GetConfiguration();

            ConfigureAuthentication(context.Services);

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

            context.Services.AddControllers()
                .AddMvcOptions(opt =>
                {
                    opt.Filters.Add(typeof(ExceptionFilter));
                }).AddNewtonsoftJson(opt =>
                {
                    var e = new StringEnumConverter();
                    opt.SerializerSettings.Converters.Add(e);
                });

            context.Services.AddHttpContextAccessor();

            context.Services.AddMicroStoreClinet(opt=> opt.BaseUrl = configuration.GetValue<string>("ShoppingApi"))
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
                    .Configure(StandardBundles.Styles.Global, bundle =>
                    {
                        bundle.AddContributors(typeof(ApplicationThemeGlobalStyleContributor));
                    });

                options
                 .ScriptBundles
                 .Configure(StandardBundles.Scripts.Global, bundle =>
                 {
                     bundle.AddContributors(typeof(ApplicationThemeGlobalScriptContributor));

                 });

                options
                    .StyleBundles
                    .Configure(StandardApplicationLayout.FrontEnd, bundle =>
                    {
                        bundle.AddContributors(typeof(FrontEndThemeStyleContributor));
                    });

                options
                   .ScriptBundles
                   .Configure(StandardApplicationLayout.FrontEnd, bundle =>
                   {
                       bundle.AddContributors(typeof(FronEndThemeScriptContributor));
                   });

                options
                    .StyleBundles
                    .Configure(StandardApplicationLayout.BackEnd, bundle =>
                    {
                        bundle.AddContributors(typeof(BackEndThemeStyleContributor));
                    });

            });

            context.Services.AddCookieManager(options =>
            {
                options.AllowEncryption = true;
                options.DefaultExpireTimeInDays = 10;
            });

            context.Services.AddFluentValidationAutoValidation();
            context.Services.AddFluentValidationClientsideAdapters();
            context.Services.AddRazorPages()
                .AddMvcOptions(opt =>
                {
                    opt.Filters.Add(typeof(PageExceptionFilter));
                })
                .AddRazorRuntimeCompilation();

  
        }

        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            using var scope = context.ServiceProvider.CreateScope();

            var storageProvider = scope.ServiceProvider.GetRequiredService<IObjectStorageProvider>();

            await storageProvider.MigrateAsync();

            await base.OnApplicationInitializationAsync(context);
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            var config = context.GetConfiguration();

            app.UseAbpRequestLocalization();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseErrorPage();
            }

            app.UseCorrelationId();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAbpRequestLocalization();

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
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
            });

        }




        private void ConfigureAuthentication(IServiceCollection services)
        {
            var appsettings = services.GetSingletonInstance<ApplicationSettings>();


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/Authentication/Login";
                options.LogoutPath = "/Authentication/Logout";
            })
           .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
           {
               options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
               options.Authority = appsettings.Security.Client.Authority;
               options.ClientId = appsettings.Security.Client.ClientId;
               options.ClientSecret = appsettings.Security.Client.ClientSecret;
               options.GetClaimsFromUserInfoEndpoint = true;
               options.UsePkce = true;
               options.ResponseType = "code";
               options.ClaimActions.MapInBoundCustomClaims();
               appsettings.Security.Client.Scopes.ForEach((scp) => options.Scope.Add(scp));
               options.SaveTokens = true;

               options.Events.OnUserInformationReceived = async (ctx) =>
               {
                   await ctx.MigrateUserBasketAsync();
               };
           });

            services.AddAccessTokenManagement();


            services.AddAuthorization(options =>
            {
                var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                var requireAdminRolePolicy = new AuthorizationPolicyBuilder()
                    .Combine(requireAuthenticatedUserPolicy)
                    .RequireClaim(ClaimTypes.Role, ApplicationSecurityRoles.Admin)
                    .Build();

                options.AddPolicy(ApplicationSecurityPolicies.RequireAuthenticatedUser, requireAuthenticatedUserPolicy);

                options.AddPolicy(ApplicationSecurityPolicies.RequireAdminRole, requireAdminRolePolicy);
            });

        }


        private void ConfigureAutoMapper() => Configure<AbpAutoMapperOptions>(opt => opt.AddMaps<PublicWebModule>());


      

    }
}
