namespace Predictor.Web.Models;

public record CalculateInput(decimal InitialBudget, MonthDate StartCalculationMonth, PaymentItem[] Incomes, PaymentItem[] Outcomes)
{
    public IEnumerable<PaymentItem> GetMonthIncomes(MonthDate month) 
        => this.Incomes.Where(x => this.Filter(month, x));

    public IEnumerable<PaymentItem> GetMonthOutcomes(MonthDate month) 
        => this.Outcomes.Where(x => this.Filter(month, x));

    private bool Filter(MonthDate month, PaymentItem item) 
        => item.StartDate == month || item.CheckRecurring(month, this.StartCalculationMonth);
}