using FluentValidation;
using Predictor.Web.Models;

namespace Predictor.Web.Validators;

public class CheckGoalRequestValidator : AbstractValidator<CheckGoalRequest>
{
    public CheckGoalRequestValidator()
    {
        this.RuleFor(x => x.Month)
            .NotNull()
            .SetValidator(new MonthDateValidator());

        this.RuleFor(x => x.PredictionId)
            .NotEmpty();

        this.When(x => x.IncomeHigherOrEqual.HasValue, () =>
        {
            this.RuleFor(x => x.IncomeHigherOrEqual!.Value)
                .GreaterThanOrEqualTo(0);
        });

        this.When(x => x.ExpenseLowerOrEqual.HasValue, () =>
        {
            this.RuleFor(x => x.ExpenseLowerOrEqual!.Value)
                .GreaterThanOrEqualTo(0);
        });
    }
}
