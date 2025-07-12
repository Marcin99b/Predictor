using Predictor.Web;
using Predictor.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure PredictorSettings
builder.Services.Configure<PredictorSettings>(
    builder.Configuration.GetSection(PredictorSettings.SectionName));

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

// Get settings
var settings = app.Services.GetRequiredService<IConfiguration>()
    .GetSection(PredictorSettings.SectionName)
    .Get<PredictorSettings>() ?? new PredictorSettings();

// Conditionally register example data endpoint
if (settings.EnableExampleData)
{
    app.MapGet("/example-data", () => ExampleData.GetCalculateInputExample(settings));
}

app.MapPost("/calc", (CalculateInput input) =>
{
    var months = new List<MonthOutput>();
    var budget = input.InitialBudget;
    var calculationPeriod = Math.Min(settings.MaxCalculationPeriodMonths, settings.MaxAllowedCalculationPeriod);
    
    foreach (var currentMonth in MonthDate.Range(input.StartCalculationMonth, calculationPeriod - 1))
    {
        var month = Calculator.CalculateMonth(input, currentMonth, budget);
        budget = month.BudgetAfter;
        months.Add(month);
    }

    return Results.Ok(new CalculationOutput([.. months]));
});

app.Run();