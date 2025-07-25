﻿using FluentValidation;
using Predictor.Web.Models;

namespace Predictor.Web.Validators;

public class PredictionRequestValidator : AbstractValidator<PredictionRequest>
{
    private const int MONTHS_IN_YEAR = 12;
    private const int MAX_YEARS_PREDICTION = 10;

    public PredictionRequestValidator()
    {
        _ = this.RuleFor(x => x.PredictionMonths)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(MONTHS_IN_YEAR * MAX_YEARS_PREDICTION);

        _ = this.RuleFor(x => x.InitialBudget)
            .GreaterThanOrEqualTo(0);

        _ = this.RuleFor(x => x.StartPredictionMonth)
            .NotNull()
            .SetValidator(new MonthDateValidator());

        _ = this.RuleForEach(x => x.Incomes)
            .NotNull()
            .SetValidator(new PaymentItemValidator());

        _ = this.RuleForEach(x => x.Expenses)
            .NotNull()
            .SetValidator(new PaymentItemValidator());
    }
}
