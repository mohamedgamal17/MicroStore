using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.ShoppingCart.Application;
using MicroStore.ShoppingCart.Infrastructure;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MicroStore.ShoppingCart.Api
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule),
            typeof(AbpAutofacModule),
            typeof(AbpAspNetCoreSerilogModule),
            typeof(MediatorModule))]
    [DependsOn(typeof(ShoppingCartApplicationModule))]
    [DependsOn(typeof(ShoppingCartInfrastructureModule))]
    public class ShoppingCartApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var host = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureAuthentication(context.Services, configuration);

            ConfigureSwagger(context.Services);
        }

        private void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
                    {
                        opt.Authority = "https://localhost:5001";
                        opt.Audience = "ShoppingCartApi";
                    });
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("BasketApi", new OpenApiInfo { Title = "Basket Api V1", Version = "v1" });
                opt.DocInclusionPredicate((docName, description) => true);
                opt.CustomSchemaIds((type) => type.FullName);
            });
        }


        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API");
                });


                app.UseHsts();

            }


            app.UseAbpRequestLocalization();

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
