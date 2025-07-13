using FluentValidation;
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
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

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

predictions.MapPost("/", (PredictionRequest request, IValidator<PredictionRequest> validator) =>
{
    validator.ValidateAndThrow(request);

    var months = new List<MonthOutput>();
    var budget = request.InitialBudget;
    foreach (var currentMonth in MonthDate.Range(request.StartCalculationMonth, request.PredictionMonths))
    {
        var month = Calculator.CalculateMonth(request, currentMonth, budget);
        budget = month.BudgetAfter;
        months.Add(month);
    }

    var monthsArray = months.ToArray();
    var summary = new BudgetSummary(
        StartingBalance: months.First().Balance,
        EndingBalance: months.Last().Balance,
        TotalIncome: months.Select(x => x.Income).Sum(),
        TotalExpenses: months.Select(x => x.Expense).Sum(),
        LowestBalance: months.OrderBy(x => x.Balance).First().Balance,
        LowestBalanceDate: months.OrderBy(x => x.Balance).First().MonthDate,
        HighestBalance: months.OrderByDescending(x => x.Balance).First().Balance,
        HighestBalanceDate: months.OrderByDescending(x => x.Balance).First().MonthDate
    );

    return new PredictionResult(summary, monthsArray);
});

predictions.MapGet("/example", () => ExampleData.CalculateInputExample);

var healthChecks = apiV1.MapGroup("/hc");
// live - Service is running.
healthChecks.MapGet("/live", () => Results.Ok());
// ready - Service can process requests correctly. Required dependencies are available etc.
healthChecks.MapGet("/ready", () => Results.Ok());

app.Run();