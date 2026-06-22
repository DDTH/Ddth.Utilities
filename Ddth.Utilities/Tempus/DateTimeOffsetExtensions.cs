namespace Ddth.Utilities.Tempus;

/// <summary>
/// Extensions that provide utility methods for the <see cref="DateTimeOffset"/> struct.
/// </summary>
public static class DateTimeOffsetExtensions
{
    /// <summary>
    /// Returns a new DateTimeOffset with the time component set to 00:00:00, preserving <see cref="DateTime.Kind"/>.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTimeOffset StartOfDay(this DateTimeOffset dateTime)
    {
#if NET6_0
        return new DateTimeOffset(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, dateTime.Offset);
#else
        return new DateTimeOffset(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, 0, dateTime.Offset);
#endif
    }

    /// <summary>
    /// Returns a new DateTimeOffset set to 00:00:00 on the first day of the week, preserving <see cref="DateTimeOffset.Offset"/>.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="firstDayOfWeek">The day considered the start of the week. Defaults to <see cref="DayOfWeek.Monday"/>.</param>
    /// <returns></returns>
    public static DateTimeOffset StartOfWeek(this DateTimeOffset dateTime, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
    {
        var diff = ((int)dateTime.DayOfWeek - (int)firstDayOfWeek + 7) % 7;
        return dateTime.AddDays(-diff).StartOfDay();
    }

    /// <summary>
    /// Returns a new DateTimeOffset set to 00:00:00 on the 1st day of the month, preserving <see cref="DateTimeOffset.Offset"/>.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTimeOffset StartOfMonth(this DateTimeOffset dateTime)
    {
#if NET6_0
        return new DateTimeOffset(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0, dateTime.Offset);
#else
        return new DateTimeOffset(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0, 0, dateTime.Offset);
#endif
    }

    /// <summary>
    /// Returns a new DateTimeOffset set to 00:00:00 on the 1st day of the quarter (Q1=Jan, Q2=Apr, Q3=Jul, Q4=Oct), preserving <see cref="DateTimeOffset.Offset"/>.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTimeOffset StartOfQuarter(this DateTimeOffset dateTime)
    {
        var quarterMonth = ((dateTime.Month - 1) / 3) * 3 + 1;
#if NET6_0
        return new DateTimeOffset(dateTime.Year, quarterMonth, 1, 0, 0, 0, 0, dateTime.Offset);
#else
        return new DateTimeOffset(dateTime.Year, quarterMonth, 1, 0, 0, 0, 0, 0, dateTime.Offset);
#endif
    }

    /// <summary>
    /// Returns a new DateTimeOffset set to 00:00:00 on January 1st of the same year, preserving <see cref="DateTimeOffset.Offset"/>.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTimeOffset StartOfCalendarYear(this DateTimeOffset dateTime)
    {
#if NET6_0
        return new DateTimeOffset(dateTime.Year, 1, 1, 0, 0, 0, 0, dateTime.Offset);
#else
        return new DateTimeOffset(dateTime.Year, 1, 1, 0, 0, 0, 0, 0, dateTime.Offset);
#endif
    }

    /// <summary>
    /// Returns a new DateTimeOffset set to 00:00:00 on the 1st day of the fiscal year, preserving <see cref="DateTimeOffset.Offset"/>.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="firstMonthOfFiscalYear">The month that starts the fiscal year (1–12). Defaults to 1 (January).</param>
    /// <returns></returns>
    public static DateTimeOffset StartOfFiscalYear(this DateTimeOffset dateTime, int firstMonthOfFiscalYear = 1)
    {
        if (firstMonthOfFiscalYear < 1 || firstMonthOfFiscalYear > 12)
            throw new ArgumentOutOfRangeException(nameof(firstMonthOfFiscalYear), firstMonthOfFiscalYear, "Value must be between 1 and 12.");
        var year = dateTime.Month >= firstMonthOfFiscalYear ? dateTime.Year : dateTime.Year - 1;
#if NET6_0
        return new DateTimeOffset(year, firstMonthOfFiscalYear, 1, 0, 0, 0, 0, dateTime.Offset);
#else
        return new DateTimeOffset(year, firstMonthOfFiscalYear, 1, 0, 0, 0, 0, 0, dateTime.Offset);
#endif
    }

    /// <summary>
    /// Returns a new DateTimeOffset representing the previous weekday (Monday to Friday) of the given DateTimeOffset.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTimeOffset PrevWeekDay(this DateTimeOffset dateTime)
    {
        var prevDay = dateTime.AddDays(-1);
        while (prevDay.DayOfWeek == DayOfWeek.Saturday || prevDay.DayOfWeek == DayOfWeek.Sunday)
        {
            prevDay = prevDay.AddDays(-1);
        }
        return prevDay;
    }

    /// <summary>
    /// Returns a new DateTimeOffset representing the next weekday (Monday to Friday) of the given DateTimeOffset.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTimeOffset NextWeekDay(this DateTimeOffset dateTime)
    {
        var nextDay = dateTime.AddDays(1);
        while (nextDay.DayOfWeek == DayOfWeek.Saturday || nextDay.DayOfWeek == DayOfWeek.Sunday)
        {
            nextDay = nextDay.AddDays(1);
        }
        return nextDay;
    }

    /// <summary>
    /// Checks if the time component of the given DateTimeOffset falls within the specified time window (e.g. start &lt;= dateTime &lt; end).
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="startInclusive"></param>
    /// <param name="endExclusive"></param>
    /// <returns></returns>
    /// <remarks>The time window can span across midnight. For example, a time window from 22:00 to 02:00 will include times from 22:00 to 23:59 and from 00:00 to 02:00.</remarks>
    public static bool IsWithinTimeWindow(this DateTimeOffset dateTime, TimeOnly startInclusive, TimeOnly endExclusive)
    {
        var time = TimeOnly.FromDateTime(dateTime.DateTime);
        return startInclusive <= endExclusive
            ? time >= startInclusive && time < endExclusive
            : time >= startInclusive || time < endExclusive;
    }

    /// <summary>
    /// Checks if the day of week component of the given DateTimeOffset falls on any of the specified days of week.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="dows"></param>
    /// <returns></returns>
    public static bool IsOnDayOfWeek(this DateTimeOffset dateTime, params DayOfWeek[] dows)
    {
        return dows.Contains(dateTime.DayOfWeek);
    }

    /// <summary>
    /// Checks if the day of week component of the given DateTimeOffset falls on any of the specified days of week.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="dows"></param>
    /// <returns></returns>
    public static bool IsOnDayOfWeek(this DateTimeOffset dateTime, IEnumerable<DayOfWeek> dows)
    {
        return dows.Contains(dateTime.DayOfWeek);
    }

    /// <summary>
    /// Checks if the day of week component of the given DateTimeOffset falls on any of the specified days of week (case-insensitive, supports both full and abbreviated day of week names).
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="dows"></param>
    /// <returns></returns>
    public static bool IsOnDayOfWeek(this DateTimeOffset dateTime, params string[] dows)
    {
        var dowStr1 = dateTime.DayOfWeek.ToString().ToUpper();
        var dowStr2 = dowStr1.Substring(0, 3);
        return dows.Where(d => d.Equals(dowStr1, StringComparison.OrdinalIgnoreCase) || d.Equals(dowStr2, StringComparison.OrdinalIgnoreCase)).Any();
    }

    /// <summary>
    /// Checks if the day of week component of the given DateTimeOffset falls on any of the specified days of week (case-insensitive, supports both full and abbreviated day of week names).
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="dows"></param>
    /// <returns></returns>
    public static bool IsOnDayOfWeek(this DateTimeOffset dateTime, IEnumerable<string> dows)
    {
        var dowStr1 = dateTime.DayOfWeek.ToString().ToUpper();
        var dowStr2 = dowStr1.Substring(0, 3);
        return dows.Where(d => d.Equals(dowStr1, StringComparison.OrdinalIgnoreCase) || d.Equals(dowStr2, StringComparison.OrdinalIgnoreCase)).Any();
    }

    /// <summary>
    /// Converts the given DateTimeOffset to the specified time zone. Returns null if the time zone ID is invalid or not found.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="tzId"></param>
    /// <returns></returns>
    public static DateTimeOffset? ToTimeZoneSilently(this DateTimeOffset dateTime, string tzId)
    {
        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(tzId);
            return TimeZoneInfo.ConvertTime(dateTime, timeZone);
        }
        catch (Exception e) when (e is TimeZoneNotFoundException or InvalidTimeZoneException)
        {
            return null;
        }
    }

    /// <summary>
    /// Shifts the given DateTimeOffset to the specified time zone (e.g. keep date and time, change only the time zone). Returns null if the time zone ID is invalid or not found.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="tzId"></param>
    /// <returns></returns>
    public static DateTimeOffset? AsTimeZoneSilently(this DateTimeOffset dateTime, string tzId)
    {
        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(tzId);
            return new DateTimeOffset(dateTime.DateTime, timeZone.GetUtcOffset(dateTime));
        }
        catch (Exception e) when (e is TimeZoneNotFoundException or InvalidTimeZoneException)
        {
            return null;
        }
    }
}
