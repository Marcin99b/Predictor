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
);