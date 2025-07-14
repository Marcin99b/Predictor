using FluentValidation;
using Predictor.Web.Models;

namespace Predictor.Web.Validators;

public class PaymentItemValidator : AbstractValidator<PaymentItem>
{
    public PaymentItemValidator()
    {
        _ = this.RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        _ = this.RuleFor(x => x.Value)
            .GreaterThan(0);

        _ = this.RuleFor(x => x.StartDate)
            .NotNull()
            .SetValidator(new MonthDateValidator());

        _ = this.When(x => x.EndDate != null, () => this.RuleFor(x => x.EndDate!)
                .NotNull()
                .SetValidator(new MonthDateValidator()));
    }
}
