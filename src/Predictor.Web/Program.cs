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

app.MapGet("/example-data", () => 
    new CalculateInput(false, 
    20_000, 
    Incomes: [
        new ("Salary", 15_000, true, MonthDate.Now),
    ],
    Outcomes: [
        new("Loan", 2_500, true, MonthDate.Now),
        new("Rent", 900, true, MonthDate.Now),
        new("Food", 1200, true, MonthDate.Now),
        new("Health Insurance", 2500, true, MonthDate.Now),
        new("Car Insurance", 300, true, MonthDate.Now),
        new("Gasoline", 500, true, MonthDate.Now),

        new("Holiday Trip", 3000, false, MonthDate.Now.AddMonths(2)),
        new("Kitchen Renovation", 20_000, false, MonthDate.Now.AddMonths(4)),

        new("Clothes", 300, false, MonthDate.Now.AddMonths(1)),
        new("Clothes", 150, false, MonthDate.Now.AddMonths(5)),
        new("Clothes", 800, false, MonthDate.Now.AddMonths(8)),

        new("Car", 120_000, false, MonthDate.Now.AddMonths(15)),
    ]));

app.MapPost("/calc", (CalculateInput input) =>
{
    var months = new List<MonthOutput>();

    var currentMonth = MonthDate.Now;
    var isFirstMonth = true;
    var budget = input.CurrentBudget;

    while (currentMonth < MonthDate.Now.AddMonths(24))
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