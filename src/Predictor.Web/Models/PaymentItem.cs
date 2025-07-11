namespace Predictor.Web.Models;

public record PaymentItem(string Name, decimal Value, MonthDate StartDate, RecurringConfig? RecurringConfig = null);
