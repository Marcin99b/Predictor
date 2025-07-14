namespace Predictor.Web.Models;

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