using FluentValidation;
using Predictor.Web.Models;

namespace Predictor.Web.Validators;

public class RecurringConfigValidator : AbstractValidator<RecurringConfig>
{
    public RecurringConfigValidator()
    {
        this.RuleFor(x => x.MonthInterval)
            .NotEmpty()
            .GreaterThanOrEqualTo(1);

        this.When(x => x.EndDate != null, () =>
        {
            this.RuleFor(x => x.EndDate!)
                .SetValidator(new MonthDateValidator());
        });
    }
}
