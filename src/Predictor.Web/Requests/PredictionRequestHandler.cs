using FluentValidation;
using MediatR;
using Predictor.Web.Models;

namespace Predictor.Web.Requests;

public class PredictionRequestHandler(IValidator<PredictionRequest> validator) : IRequestHandler<PredictionRequest, PredictionResult>
{
    public Task<PredictionResult> Handle(PredictionRequest request, CancellationToken cancellationToken)
    {
        validator.ValidateAndThrow(request);

        var months = new List<MonthOutput>();
        var budget = request.InitialBudget;
        foreach (var currentMonth in MonthDate.Range(request.StartPredictionMonth, request.PredictionMonths))
        {
            var month = Calculator.CalculateMonth(request, currentMonth, budget);
            budget = month.BudgetAfter;
            months.Add(month);
        }

        var monthsArray = months.ToArray();
        var summary = new BudgetSummary(
            StartingBalance: months.First().Balance,
            EndingBalance: months.Last().Balance,
            TotalIncome: months.Select(x => x.Income).Sum(),
            TotalExpenses: months.Select(x => x.Expense).Sum(),
            LowestBalance: months.OrderBy(x => x.Balance).First().Balance,
            LowestBalanceDate: months.OrderBy(x => x.Balance).First().MonthDate,
            HighestBalance: months.OrderByDescending(x => x.Balance).First().Balance,
            HighestBalanceDate: months.OrderByDescending(x => x.Balance).First().MonthDate
        );

        var result = new PredictionResult(summary, monthsArray);
        return Task.FromResult(result);
    }
}
