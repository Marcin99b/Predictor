using Predictor.Web.Models;

namespace Predictor.Web;

//static temporary. it should be registered
public static class Calculator
{
    public static MonthOutput CalculateMonth(PredictionRequest input, MonthDate month, decimal budgetBefore)
    {
        var currentMonthIncome = input.GetMonthIncomes(month).Sum(x => x.Value);
        var currentMonthExpense = input.GetMonthExpenses(month).Sum(x => x.Value);

        var balance = currentMonthIncome - currentMonthExpense;
        var budgetAfter = budgetBefore + balance;

        return new MonthOutput(month, budgetAfter, balance, currentMonthIncome, currentMonthExpense);
    }
}
