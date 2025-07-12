using FluentValidation;
using Microsoft.VisualBasic;
using Predictor.Web;
using Predictor.Web.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

app.MapGet("/example-data", () => ExampleData.CalculateInputExample);

app.MapPost("/calc", (CalculateInput input, IValidator<CalculateInput> validator) =>
{
    validator.ValidateAndThrow(input);

    var months = new List<MonthOutput>();
    var budget = input.InitialBudget;
    foreach (var currentMonth in MonthDate.Range(input.StartCalculationMonth, 3))
    {
        var month = Calculator.CalculateMonth(input, currentMonth, budget);
        budget = month.BudgetAfter;
        months.Add(month);
    }

    return new CalculationOutput([.. months]);
});

// live - Service is running.
app.MapGet("/hc/live", () => Results.Ok());
// ready - Service can process requests correctly. Required dependencies are available etc.
app.MapGet("/hc/ready", () => Results.Ok());

app.Run();