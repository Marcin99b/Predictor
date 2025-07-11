namespace Predictor.Web.Models;

public record CalculateInput(decimal InitialBudget, MonthDate StartCalculationMonth, PaymentItem[] Incomes, PaymentItem[] Outcomes)
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

    public IEnumerable<PaymentItem> GetMonthIncomes(MonthDate month)
    {
        return this.Incomes.Where(x => x.StartDate == month || this.CheckRecurring(month, x.StartDate, x.RecurringConfig));
    }

    public IEnumerable<PaymentItem> GetMonthOutcomes(MonthDate month)
    {
        return this.Outcomes.Where(x => x.StartDate == month || this.CheckRecurring(month, x.StartDate, x.RecurringConfig));
    }
}