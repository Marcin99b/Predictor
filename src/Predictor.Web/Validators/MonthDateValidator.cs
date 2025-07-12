using FluentValidation;
using Predictor.Web.Models;

namespace Predictor.Web.Validators;

public class MonthDateValidator : AbstractValidator<MonthDate>
{
    public MonthDateValidator()
    {
        this.RuleFor(x => x.Month)
            .NotEmpty()
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(12);

        this.RuleFor(x => x.Year)
            .NotEmpty()
            .GreaterThanOrEqualTo(1900)
            .LessThanOrEqualTo(2999);
    }
}
