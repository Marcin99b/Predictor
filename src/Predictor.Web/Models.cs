
public record CalculateInput(decimal InitialBudget, IncomeItem[] Incomes, OutcomeItem[] Outcomes)
{
    public IEnumerable<IncomeItem> GetMonthIncomes(MonthDate month)
    {
        return this.Incomes.Where(x => x.StartDate == month || x.IsRecurring && x.StartDate < month);
    }

    public IEnumerable<OutcomeItem> GetMonthOutcomes(MonthDate month)
    {
        return this.Outcomes.Where(x => x.StartDate == month || x.IsRecurring && x.StartDate < month);
    }
}

public record CalculationOutput(MonthOutput[] Months);
public record MonthOutput(MonthDate MonthDate, decimal BudgetAfter, decimal Balance, decimal Income, decimal Outcome);

public record IncomeItem(string Name, decimal Value, bool IsRecurring, MonthDate StartDate);
public record OutcomeItem(string Name, decimal Value, bool IsRecurring, MonthDate StartDate);

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
        yield return current;
        for (int i = 0; i < monthsAhead; i++) 
        {
            current = current.AddMonths(i);
            yield return current;
        }
    }
}
