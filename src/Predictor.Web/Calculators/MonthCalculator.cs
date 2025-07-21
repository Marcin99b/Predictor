using Predictor.Web.Models;
using Predictor.Web.Services;

namespace Predictor.Web.Calculators;
public class MonthCalculator(ICurrencyService currencyService)
{
    private readonly ICurrencyService _currencyService = currencyService;

    public async Task<MonthOutput> CalculateMonthAsync(PredictionRequest input, MonthDate month, decimal budgetBefore)
    {
        var incomeTasks = input.GetMonthIncomes(month)
            .Select(x => this.ConvertValue(x, input.OutputCurrency));
        var incomeValues = await Task.WhenAll(incomeTasks);
        var income = incomeValues.Sum();

        var expenseTasks = input.GetMonthExpenses(month)
            .Select(x => this.ConvertValue(x, input.OutputCurrency));
        var expenseValues = await Task.WhenAll(expenseTasks);
        var expense = expenseValues.Sum();

        var balance = income - expense;
        var budgetAfter = budgetBefore + balance;

        return new MonthOutput(month, budgetAfter, balance, income, expense);
    }

    private async Task<decimal> ConvertValue(PaymentItem item, string outputCurrency)
    {
        if (string.IsNullOrWhiteSpace(item.Currency) ||
            string.IsNullOrWhiteSpace(outputCurrency) ||
            item.Currency.Trim().Equals(outputCurrency.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            return item.Value;
        }

        var exchangeRate = await this._currencyService.GetExchangeRateAsync(item.Currency, outputCurrency);
        return item.Value * exchangeRate;
    }
}
