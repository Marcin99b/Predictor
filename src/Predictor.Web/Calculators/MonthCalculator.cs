using Predictor.Web.Models;

namespace Predictor.Web.Calculators;

public class MonthCalculator
{
    public MonthOutput CalculateMonth(PredictionRequest input, MonthDate month, decimal budgetBefore)
    {
        var income = input.GetMonthIncomes(month).Sum(x => x.Value);
        var expense = input.GetMonthExpenses(month).Sum(x => x.Value);

        var balance = income - expense;
        var budgetAfter = budgetBefore + balance;

        return new MonthOutput(month, budgetAfter, balance, income, expense);
    }
}
