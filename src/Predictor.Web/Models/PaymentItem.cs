namespace Predictor.Web.Models;

public record PaymentItem(string Name, decimal Value, MonthDate StartDate, Frequency Frequency = Frequency.OneTime, MonthDate? EndDate = null)
{
    public bool Check(MonthDate month)
    {
        if (this.EndDate != null && this.EndDate < this.StartDate)
        {
            return false;
        }

        if (this.StartDate > month)
        {
            return false;
        }

        if (this.EndDate != null && month > this.EndDate)
        {
            return false;
        }

        if (month == this.StartDate)
        {
            return true;
        }

        if (this.Frequency == Frequency.OneTime)
        {
            return false;
        }

        var calculatedMonth = this.StartDate;
        var monthInterval = this.Frequency switch
        {
            Frequency.Monthly => 1,
            Frequency.Quarterly => 3,
            Frequency.SemiAnnually => 6,
            Frequency.Annually => 12,
            _ => throw new NotImplementedException()
        };

        while (calculatedMonth < month)
        {
            calculatedMonth = calculatedMonth.AddMonths(monthInterval);
        }

        return calculatedMonth == month;
    }
}