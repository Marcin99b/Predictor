using Predictor.Web;

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

    var currentMonth = MonthDate.Now;
    var isFirstMonth = true;
    var budget = input.CurrentBudget;

    while (currentMonth < MonthDate.Now.AddMonths(12 * 3))
    {
        var currentMonthIncome = input.GetMonthIncomes(currentMonth).Sum(x => x.Value);
        var currentMonthOutcome = input.GetMonthOutcomes(currentMonth).Sum(x => x.Value);

        var balance = currentMonthIncome - currentMonthOutcome;

        if (!isFirstMonth || isFirstMonth && input.CalculateCurrentMonth)
        {
            budget += balance;
        }

        if (isFirstMonth)
        {
            isFirstMonth = false;
        }

        var month = new MonthOutput(currentMonth, budget, balance, currentMonthIncome, currentMonthOutcome);
        months.Add(month);

        currentMonth = currentMonth.AddMonths(1);
    }

    return new CalculationOutput([.. months]);
});

app.Run();