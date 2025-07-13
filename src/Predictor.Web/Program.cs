using FluentValidation;
using MediatR;
using Microsoft.OpenApi.Models;
using Predictor.Web;
using Predictor.Web.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
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
builder.Services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var apiV1 = app.MapGroup("/api/v1");
var predictions = apiV1.MapGroup("/predictions");

predictions.MapPost("/", (PredictionRequest request, IMediator mediator) => mediator.Send(request));

predictions.MapGet("/example", () => ExampleData.CalculateInputExample);

var healthChecks = apiV1.MapGroup("/hc");
// live - Service is running.
healthChecks.MapGet("/live", () => Results.Ok());
// ready - Service can process requests correctly. Required dependencies are available etc.
healthChecks.MapGet("/ready", () => Results.Ok());

app.Run();