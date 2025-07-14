using FluentValidation;
using Predictor.Web.Models;

namespace Predictor.Web.Validators;

public class CheckGoalRequestValidator : AbstractValidator<CheckGoalRequest>
{
    public CheckGoalRequestValidator()
    {
        _ = this.RuleFor(x => x.Month)
            .NotNull()
            .SetValidator(new MonthDateValidator());

        _ = this.RuleFor(x => x.PredictionId)
            .NotEmpty();

        _ = this.When(x => x.IncomeHigherOrEqual.HasValue, () => this.RuleFor(x => x.IncomeHigherOrEqual!.Value)
                .GreaterThanOrEqualTo(0));

        _ = this.When(x => x.ExpenseLowerOrEqual.HasValue, () => this.RuleFor(x => x.ExpenseLowerOrEqual!.Value)
                .GreaterThanOrEqualTo(0));
    }
}
