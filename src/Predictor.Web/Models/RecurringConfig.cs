using Predictor.Web.Models;

public record RecurringConfig(int MonthInterval, MonthDate? EndDate = null);
