namespace Predictor.Web.Models;

public record BudgetSummary(
    decimal StartingBalance,
    decimal EndingBalance,
    decimal TotalIncome,
    decimal TotalExpenses,
    decimal LowestBalance,
    MonthDate LowestBalanceDate,
    decimal HighestBalance,
    MonthDate HighestBalanceDate
)
{
    public static BudgetSummary FromMonths(MonthOutput[] months)
    {
        if (months == null || months.Length == 0)
        {
            throw new ArgumentException("Months cannot be null or empty.", nameof(months));
        }

        return new BudgetSummary(
            StartingBalance: months.First().Balance,
            EndingBalance: months.Last().Balance,
            TotalIncome: months.Select(x => x.Income).Sum(),
            TotalExpenses: months.Select(x => x.Expense).Sum(),
            LowestBalance: months.OrderBy(x => x.Balance).First().Balance,
            LowestBalanceDate: months.OrderBy(x => x.Balance).First().MonthDate,
            HighestBalance: months.OrderByDescending(x => x.Balance).First().Balance,
            HighestBalanceDate: months.OrderByDescending(x => x.Balance).First().MonthDate
        );
    }
}