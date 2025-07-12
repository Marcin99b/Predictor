using FluentValidation;
using Predictor.Web.Models;

namespace Predictor.Web.Validators;

public class PaymentItemValidator : AbstractValidator<PaymentItem>
{
    public PaymentItemValidator()
    {
        this.RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        this.RuleFor(x => x.Value)
            .NotEmpty()
            .GreaterThan(0);

        this.RuleFor(x => x.StartDate)
            .NotNull()
            .SetValidator(new MonthDateValidator());

        this.When(x => x.RecurringConfig != null, () => 
        {
            this.RuleFor(x => x.RecurringConfig!)
                .SetValidator(new RecurringConfigValidator());
        });
    }
}
