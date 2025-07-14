using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Predictor.Web.Integrations;
using System.Diagnostics.CodeAnalysis;

namespace Predictor.Web;

[ExcludeFromCodeCoverage]
public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        _ = builder.Services.AddControllers();
        _ = builder.Services.AddEndpointsApiExplorer();
        _ = builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Predictor API",
                Version = "v1",
                Description = "API for predicting budget scenarios"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });
        });

        var assembly = typeof(Program).Assembly;
        _ = builder.Services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
        _ = builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        _ = builder.Services.AddHealthChecks();

        _ = builder.Services.AddMemoryCache();
        _ = builder.Services.AddSingleton<CacheRepository>();

        _ = builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        _ = builder.Services.AddProblemDetails();

        var app = builder.Build();

        _ = app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI();
        }

        _ = app.UseHttpsRedirection();
        _ = app.UseAuthorization();
        _ = app.MapControllers();

        _ = app.MapGroup("/api/v1").MapPredictionsV1().MapAnalyticsV1();
        _ = app.MapHealthChecks("/hc/ready", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Tags.Contains("ready")
        });

        _ = app.MapHealthChecks("/hc/live", new HealthCheckOptions
        {
            Predicate = _ => false
        });

        app.Run();
    }
}
