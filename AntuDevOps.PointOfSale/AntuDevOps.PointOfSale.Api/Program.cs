using AntuDevOps.AspNetCore.Http.Problems.DependencyInjection;
using AntuDevOps.PointOfSale.Api.Errors;
using AntuDevOps.PointOfSale.Api.Logging;
using AntuDevOps.PointOfSale.Api.OAuth;
using AntuDevOps.PointOfSale.Application.DependencyInjection;
using AntuDevOps.PointOfSale.Domain.Exceptions;
using AntuDevOps.PointOfSale.Infrastructure.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, config) =>
{
    config
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Sink(new SQLServerSink(services));
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(cors => cors
    .AddDefaultPolicy(policy => policy
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()));

builder.Services
    .AddJwtService(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddErrorResponse(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddProblemDetails(x => x
    .WithConfiguration(builder.Configuration)
    .AddProblemDetailsFactory<DomainException, DomainExceptionProblemDetailsFactory>()
    .AddProblemDetailsFactory<NotFoundException, NotFoundExceptionProblemDetailsFactory>()
    );

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseSerilogRequestLogging(options =>
{
    options.IncludeQueryInRequestPath = true;
});

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
