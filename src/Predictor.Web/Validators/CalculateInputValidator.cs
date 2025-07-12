using FluentValidation;
using Predictor.Web.Models;

namespace Predictor.Web.Validators;

public class CalculateInputValidator : AbstractValidator<CalculateInput>
{
    public CalculateInputValidator()
    {
        this.RuleFor(x => x.InitialBudget)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);

        this.RuleFor(x => x.StartCalculationMonth)
            .NotNull()
            .SetValidator(new MonthDateValidator());

        this.RuleForEach(x => x.Incomes)
            .NotNull()
            .SetValidator(new PaymentItemValidator());

        this.RuleForEach(x => x.Outcomes)
            .NotNull()
            .SetValidator(new PaymentItemValidator());
    }
}
