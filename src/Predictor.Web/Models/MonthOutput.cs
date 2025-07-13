namespace Predictor.Web.Models;
  
public record MonthOutput(MonthDate MonthDate, decimal BudgetAfter, decimal Balance, decimal Income, decimal Expense);