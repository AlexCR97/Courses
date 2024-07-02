using AntuDevOps.AspNetCore.Http.Problems.DependencyInjection;
using AntuDevOps.PointOfSale.Api.Errors;
using AntuDevOps.PointOfSale.Api.Filters;
using AntuDevOps.PointOfSale.Api.Middlewares;
using AntuDevOps.PointOfSale.Api.OAuth;
using AntuDevOps.PointOfSale.Application.DependencyInjection;
using AntuDevOps.PointOfSale.Domain.Exceptions;
using AntuDevOps.PointOfSale.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    //options.Filters.Add<ExceptionFilter>();
});

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

//builder.Services.AddSingleton<ExceptionMiddleware>();

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

//app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

//app.UseExceptionHandler("/error");

app.UseExceptionHandler();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
