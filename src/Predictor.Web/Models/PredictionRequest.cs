namespace Predictor.Web.Models;

public record PredictionRequest(int PredictionMonths, decimal InitialBudget, MonthDate StartPredictionMonth, PaymentItem[] Incomes, PaymentItem[] Expenses)
{
    public IEnumerable<PaymentItem> GetMonthIncomes(MonthDate month) 
        => this.Incomes.Where(x => this.Filter(month, x));

    public IEnumerable<PaymentItem> GetMonthExpenses(MonthDate month) 
        => this.Expenses.Where(x => this.Filter(month, x));

    private bool Filter(MonthDate month, PaymentItem item) 
        => item.StartDate == month || item.CheckRecurring(month);
}