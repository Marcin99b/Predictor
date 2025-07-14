using FluentValidation;
using MediatR;
using Predictor.Web.Integrations;
using Predictor.Web.Models;

namespace Predictor.Web.Handlers;

public class CheckGoalRequestHandler(CacheRepository cache, IValidator<CheckGoalRequest> validator) : IRequestHandler<CheckGoalRequest, bool>
{
    public Task<bool> Handle(CheckGoalRequest request, CancellationToken cancellationToken)
    {
        validator.Validate(request);

        var prediction = cache.Get_PredictionResult(request.PredictionId);
        var month = prediction?.Months?.FirstOrDefault(x => x.MonthDate == request.Month);
        if (month == null)
        {
            //todo what if prediction is too short?
            return Task.FromResult(false);
        }

        if (request.BalanceHigherOrEqual.HasValue && month.Balance < request.BalanceHigherOrEqual.Value)
        {
            return Task.FromResult(false);
        }

        if (request.IncomeHigherOrEqual.HasValue && month.Income < request.IncomeHigherOrEqual) 
        {
            return Task.FromResult(false);
        }

        if (request.ExpenseLowerOrEqual.HasValue && month.Expense > request.ExpenseLowerOrEqual)
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }
}
