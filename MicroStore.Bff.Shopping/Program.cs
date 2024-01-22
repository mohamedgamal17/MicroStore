using MicroStore.Bff.Shopping;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddBffShopping(builder.Configuration);


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();