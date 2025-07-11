namespace Predictor.Web.Models;

public record PaymentItem(string Name, decimal Value, MonthDate StartDate, RecurringConfig? RecurringConfig = null)
{
    public bool CheckRecurring(MonthDate month, MonthDate startCalculationMonth)
    {
        if (this.RecurringConfig == null || this.StartDate > month)
        {
            return false;
        }

        if (this.RecurringConfig.EndDate != null && this.RecurringConfig.EndDate < month)
        {
            return false;
        }

        var calculatedMonth = startCalculationMonth;
        while (calculatedMonth < month)
        {
            calculatedMonth = calculatedMonth.AddMonths(this.RecurringConfig.MonthInterval);
        }

        return calculatedMonth == month;
    }
}
