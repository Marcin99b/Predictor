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
    new CalculationInput(20_000, 
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

app.MapPost("/calc", (CalculationInput input) => 
{
    var months = new List<MonthOutput>();
    return new CalculationOutput([.. months]);
});

app.Run();

public record CalculationInput(decimal CurrentBudget, IncomeItem[] Incomes, OutcomeItem[] Outcomes);
public record CalculationOutput(MonthOutput[] Months);
public record MonthOutput();

public record IncomeItem(string Name, decimal Value, bool IsRecurring, MonthDate StartDate);
public record OutcomeItem(string Name, decimal Value, bool IsRecurring, MonthDate StartDate);

public readonly struct MonthDate(int month, int year)
{
    public int Month { get; } = month;
    public int Year { get; } = year;

    public static MonthDate Now => new (DateTime.Now.Month, DateTime.Now.Year);
    public MonthDate AddMonths(int months)
    {
        var month = this.Month + months;
        var year = this.Year;
        while (month > 12)
        {
            month -= 12;
            year++;
        }

        return new MonthDate(month, year);
    }
}
