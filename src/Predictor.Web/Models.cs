
public record CalculateInput(decimal InitialBudget, MonthDate StartCalculationMonth, IncomeItem[] Incomes, OutcomeItem[] Outcomes)
{
    private bool CheckRecurring(MonthDate month, MonthDate StartDate, RecurringConfig? recurringConfig)
    {
        if (recurringConfig == null || StartDate > month)
        {
            return false;
        }

        if (recurringConfig.EndDate != null && recurringConfig.EndDate < month)
        {
            return false;
        }

        var calculatedMonth = this.StartCalculationMonth;
        while (calculatedMonth < month)
        {
            calculatedMonth = calculatedMonth.AddMonths(recurringConfig.MonthInterval);
        }

        return calculatedMonth == month;
    }

    public IEnumerable<IncomeItem> GetMonthIncomes(MonthDate month)
    {
        return this.Incomes.Where(x => x.StartDate == month || this.CheckRecurring(month, x.StartDate, x.RecurringConfig));
    }

    public IEnumerable<OutcomeItem> GetMonthOutcomes(MonthDate month)
    {
        return this.Outcomes.Where(x => x.StartDate == month || this.CheckRecurring(month, x.StartDate, x.RecurringConfig));
    }
}

public record CalculationOutput(MonthOutput[] Months);
public record MonthOutput(MonthDate MonthDate, decimal BudgetAfter, decimal Balance, decimal Income, decimal Outcome);

/// <summary>
/// </summary>
/// <param name="Name"></param>
/// <param name="Value"></param>
/// <param name="StartDate"></param>
/// <param name="RecurringConfig">Null mean one time payment</param>
public record IncomeItem(string Name, decimal Value, MonthDate StartDate, RecurringConfig? RecurringConfig = null)
{
    public bool IsRecurring { get; } = RecurringConfig != null;
}

/// <summary>
/// </summary>
/// <param name="Name"></param>
/// <param name="Value"></param>
/// <param name="StartDate"></param>
/// <param name="RecurringConfig">Null mean one time payment</param>
public record OutcomeItem(string Name, decimal Value, MonthDate StartDate, RecurringConfig? RecurringConfig = null)
{
    public bool IsRecurring { get; } = RecurringConfig != null;
}

/// <summary>
/// </summary>
/// <param name="MonthInterval"></param>
/// <param name="EndDate">Null mean infinite interval</param>
public record RecurringConfig(int MonthInterval, MonthDate? EndDate = null);

public record MonthDate(int Month, int Year)
{
    public static MonthDate Now => new(DateTime.Now.Month, DateTime.Now.Year);
    public MonthDate AddMonths(int months)
    {
        var month = this.Month + months;
        var year = this.Year;
        while (month > 12)
        {
            month -= 12;
            year++;
        }

        return new MonthDate(month, year);
    }

    public static bool operator <(MonthDate a, MonthDate b)
    {
        if (a.Year < b.Year)
        {
            return true;
        }

        if (a.Year > b.Year)
        {
            return false;
        }

        return a.Month < b.Month;
    }

    public static bool operator >(MonthDate a, MonthDate b)
    {
        if (a.Year > b.Year)
        {
            return true;
        }

        if (a.Year < b.Year)
        {
            return false;
        }

        return a.Month > b.Month;
    }

    /// <summary>
    /// Return passed month and {monthsAhead} next months. 
    /// For example ("January", 2) returns ["January", "February", "March"]
    /// </summary>
    /// <param name="from"></param>
    /// <param name="monthsAhead"></param>
    /// <returns></returns>
    public static IEnumerable<MonthDate> Range(MonthDate from, int monthsAhead)
    {
        var current = from;
        for (int i = 0; i <= monthsAhead; i++) 
        {
            current = current.AddMonths(1);
            yield return current;
        }
    }
}
