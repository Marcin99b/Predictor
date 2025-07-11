using Predictor.Web.Models;

public record OutcomeItem(string Name, decimal Value, MonthDate StartDate, RecurringConfig? RecurringConfig = null)
{
    public bool IsRecurring { get; } = RecurringConfig != null;
}
