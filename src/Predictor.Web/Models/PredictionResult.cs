namespace Predictor.Web.Models;

public record PredictionResult(Guid Id, BudgetSummary Summary, MonthOutput[] Months);
