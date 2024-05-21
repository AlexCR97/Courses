using AntuDevOps.PointOfSale.Api.ApiVersioning;
using AntuDevOps.PointOfSale.Application.DependencyInjection;
using AntuDevOps.PointOfSale.Infrastructure.DependencyInjection;
using Asp.Versioning.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomApiVersioning();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        options.AddApiVersionedSwaggerEndpoints(apiVersionDescriptionProvider);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
