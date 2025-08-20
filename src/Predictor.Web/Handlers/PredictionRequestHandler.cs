using FluentValidation;
using MediatR;
using Predictor.Web.Calculators;
using Predictor.Web.Integrations;
using Predictor.Web.Models;

namespace Predictor.Web.Handlers;

public class PredictionRequestHandler(
    IValidator<PredictionRequest> validator, 
    CacheRepository cache, 
    MonthCalculator calculator) 
    : IRequestHandler<PredictionRequest, PredictionResult>
{
    public async Task<PredictionResult> Handle(PredictionRequest request, CancellationToken cancellationToken)
    {
        validator.ValidateAndThrow(request);

        var months = new List<MonthOutput>();
        var budget = request.InitialBudget;
        foreach (var currentMonth in MonthDate.Range(request.StartPredictionMonth, request.PredictionMonths))
        {
            var month = await calculator.CalculateMonthAsync(request, currentMonth, budget);
            budget = month.BudgetAfter;
            months.Add(month);
        }

        var monthsArray = months.ToArray();
        var summary = BudgetSummary.FromMonths(monthsArray);

        //todo check if PutId already exists and is owned by user
        //protect from editing someone else data
        var result = new PredictionResult(request.PutId ?? Guid.NewGuid(), summary, monthsArray);
        cache.Set_PredictionResult(result);

        return await Task.FromResult(result);
    }
}
