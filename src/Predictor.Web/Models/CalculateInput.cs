namespace Predictor.Web.Models;

public record CalculateInput(decimal InitialBudget, MonthDate StartCalculationMonth, IncomeItem[] Incomes, OutcomeItem[] Outcomes)
{
    private bool CheckRecurring(MonthDate month, MonthDate StartDate, RecurringConfig? recurringConfig)
    {
        if (recurringConfig == null || StartDate > month)
        {
            return false;
        }

        if (recurringConfig.EndDate != null && recurringConfig.EndDate < month)
        {
            return false;
        }

        var calculatedMonth = this.StartCalculationMonth;
        while (calculatedMonth < month)
        {
            calculatedMonth = calculatedMonth.AddMonths(recurringConfig.MonthInterval);
        }

        return calculatedMonth == month;
    }

    public IEnumerable<IncomeItem> GetMonthIncomes(MonthDate month)
    {
        return this.Incomes.Where(x => x.StartDate == month || this.CheckRecurring(month, x.StartDate, x.RecurringConfig));
    }

    public IEnumerable<OutcomeItem> GetMonthOutcomes(MonthDate month)
    {
        return this.Outcomes.Where(x => x.StartDate == month || this.CheckRecurring(month, x.StartDate, x.RecurringConfig));
    }
}