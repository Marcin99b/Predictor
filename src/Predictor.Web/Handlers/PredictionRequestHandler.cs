using FluentValidation;
using MediatR;
using Predictor.Web.Integrations;
using Predictor.Web.Models;

namespace Predictor.Web.Handlers;

public class PredictionRequestHandler(IValidator<PredictionRequest> validator, CacheRepository cache) : IRequestHandler<PredictionRequest, PredictionResult>
{
    public Task<PredictionResult> Handle(PredictionRequest request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

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

        //todo check if PutId already exists and is owned by user
        //protect from editing someone else data
        var result = new PredictionResult(request.PutId ?? Guid.NewGuid(), summary, monthsArray);
        cache.Set_PredictionResult(result);

        return Task.FromResult(result);
    }
}
