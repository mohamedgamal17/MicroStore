using MicroStore.Bff.Shopping;
using MicroStore.Bff.Shopping.Config;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddBffShopping(builder.Configuration);


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    var securityConfiguration = app.Services.GetRequiredService<SecurityConfiguration>();

    app.UseSwagger();

    app.UseSwaggerUI(opt =>
    {
        if(securityConfiguration.SwaggerClient != null)
        {
            opt.OAuthClientId(securityConfiguration.SwaggerClient.Id);
            opt.OAuthClientSecret(securityConfiguration.SwaggerClient.Secret);
            opt.OAuthScopeSeparator(" ");
            opt.OAuthUsePkce();
        }
       
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();