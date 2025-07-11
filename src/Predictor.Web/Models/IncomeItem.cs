namespace Predictor.Web.Models;

public record IncomeItem(string Name, decimal Value, MonthDate StartDate, RecurringConfig? RecurringConfig = null)
{
    public bool IsRecurring { get; } = RecurringConfig != null;
}