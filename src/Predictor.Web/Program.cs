using Predictor.Web;
using Predictor.Web.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapPost("/calc", (CalculateInput input) =>
{
    var months = new List<MonthOutput>();
    var budget = input.InitialBudget;
    foreach (var currentMonth in MonthDate.Range(input.StartCalculationMonth, 12 * 3 - 1))
    {
        var month = Calculator.CalculateMonth(input, currentMonth, budget);
        budget = month.BudgetAfter;
        months.Add(month);
    }

    return new CalculationOutput([.. months]);
});

app.Run();