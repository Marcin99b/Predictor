namespace Predictor.Web.Models;

public record PredictionResult(BudgetSummary Summary, MonthOutput[] Months);
