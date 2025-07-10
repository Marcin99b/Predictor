
public record CalculateInput(bool CalculateCurrentMonth, decimal CurrentBudget, IncomeItem[] Incomes, OutcomeItem[] Outcomes)
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
public record MonthOutput(MonthDate MonthDate, decimal Budget, decimal Balance, decimal Income, decimal Outcome);

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
}
