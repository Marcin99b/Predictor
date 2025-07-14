using FluentValidation;
using MediatR;
using Predictor.Web.Integrations;
using Predictor.Web.Models;

namespace Predictor.Web.Handlers;

public class CheckGoalRequestHandler(CacheRepository cache, IValidator<CheckGoalRequest> validator) : IRequestHandler<CheckGoalRequest, bool>
{
    public Task<bool> Handle(CheckGoalRequest request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var prediction = cache.Get_PredictionResult(request.PredictionId);
        var month = prediction?.Months?.FirstOrDefault(x => x.MonthDate == request.Month);
        if (month == null)
        {
            //todo what if prediction is too short?
            return Task.FromResult(false);
        }

        return request.BalanceHigherOrEqual.HasValue && month.Balance < request.BalanceHigherOrEqual.Value
            ? Task.FromResult(false)
            : request.IncomeHigherOrEqual.HasValue && month.Income < request.IncomeHigherOrEqual
            ? Task.FromResult(false)
            : request.ExpenseLowerOrEqual.HasValue && month.Expense > request.ExpenseLowerOrEqual
            ? Task.FromResult(false)
            : Task.FromResult(true);
    }
}
