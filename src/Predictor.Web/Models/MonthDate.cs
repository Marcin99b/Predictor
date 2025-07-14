namespace Predictor.Web.Models;

public record MonthDate(int Month, int Year)
{
    public static MonthDate Now => new(DateTime.Now.Month, DateTime.Now.Year);
    public MonthDate AddMonths(int months)
    {
        if (months <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(months), "Months to add must be greater than zero.");
        }

        var totalMonths = this.Year * 12 + this.Month - 1 + months;

        var newYear = totalMonths / 12;
        var newMonth = totalMonths % 12 + 1;

        return new MonthDate(newMonth, newYear);
    }

    public static bool operator <(MonthDate a, MonthDate b) => a.Year < b.Year || a.Year <= b.Year && a.Month < b.Month;

    public static bool operator >(MonthDate a, MonthDate b) => a.Year > b.Year || a.Year >= b.Year && a.Month > b.Month;

    public static IEnumerable<MonthDate> Range(MonthDate from, int monthsCount)
    {
        var current = from;
        yield return current;
        for (var i = 0; i < monthsCount - 1; i++)
        {
            current = current.AddMonths(1);
            yield return current;
        }
    }
}