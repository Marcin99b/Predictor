using MediatR;

namespace Predictor.Web.Models;

public record CheckGoalRequest(
    Guid PredictionId,
    MonthDate Month,
    decimal? BalanceHigherOrEqual = null,
    decimal? IncomeHigherOrEqual = null,
    decimal? ExpenseLowerOrEqual = null)
    : IRequest<bool>;