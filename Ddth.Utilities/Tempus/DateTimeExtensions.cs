namespace Ddth.Utilities.Tempus;

/// <summary>
/// Extensions that provide utility methods for the <see cref="DateTime"/> struct.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Returns a new DateTime with the time component set to 00:00:00, preserving <see cref="DateTime.Kind"/>.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime StartOfDay(this DateTime dateTime)
    {
#if NET6_0
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, dateTime.Kind);
#else
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, 0, dateTime.Kind);
#endif
    }

    /// <summary>
    /// Returns a new DateTime set to 00:00:00 on the first day of the week, preserving <see cref="DateTime.Kind"/>.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="firstDayOfWeek">The day considered the start of the week. Defaults to <see cref="DayOfWeek.Monday"/>.</param>
    /// <returns></returns>
    public static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
    {
        var diff = ((int)dateTime.DayOfWeek - (int)firstDayOfWeek + 7) % 7;
        return dateTime.AddDays(-diff).StartOfDay();
    }

    /// <summary>
    /// Returns a new DateTime set to 00:00:00 on the 1st day of the month, preserving <see cref="DateTime.Kind"/>.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime StartOfMonth(this DateTime dateTime)
    {
#if NET6_0
        return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0, dateTime.Kind);
#else
        return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0, 0, dateTime.Kind);
#endif
    }

    /// <summary>
    /// Returns a new DateTime set to 00:00:00 on the 1st day of the quarter (Q1=Jan, Q2=Apr, Q3=Jul, Q4=Oct), preserving <see cref="DateTime.Kind"/>.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime StartOfQuarter(this DateTime dateTime)
    {
        var quarterMonth = ((dateTime.Month - 1) / 3) * 3 + 1;
#if NET6_0
        return new DateTime(dateTime.Year, quarterMonth, 1, 0, 0, 0, 0, dateTime.Kind);
#else
        return new DateTime(dateTime.Year, quarterMonth, 1, 0, 0, 0, 0, 0, dateTime.Kind);
#endif
    }

    /// <summary>
    /// Returns a new DateTime set to 00:00:00 on January 1st of the same year, preserving <see cref="DateTime.Kind"/>.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime StartOfCalendarYear(this DateTime dateTime)
    {
#if NET6_0
        return new DateTime(dateTime.Year, 1, 1, 0, 0, 0, 0, dateTime.Kind);
#else
        return new DateTime(dateTime.Year, 1, 1, 0, 0, 0, 0, 0, dateTime.Kind);
#endif
    }

    /// <summary>
    /// Returns a new DateTime representing the previous weekday (Monday to Friday) of the given DateTime.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime PrevWeekDay(this DateTime dateTime)
    {
        var prevDay = dateTime.AddDays(-1);
        while (prevDay.DayOfWeek == DayOfWeek.Saturday || prevDay.DayOfWeek == DayOfWeek.Sunday)
        {
            prevDay = prevDay.AddDays(-1);
        }
        return prevDay;
    }

    /// <summary>
    /// Returns a new DateTime representing the next weekday (Monday to Friday) of the given DateTime.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime NextWeekDay(this DateTime dateTime)
    {
        var nextDay = dateTime.AddDays(1);
        while (nextDay.DayOfWeek == DayOfWeek.Saturday || nextDay.DayOfWeek == DayOfWeek.Sunday)
        {
            nextDay = nextDay.AddDays(1);
        }
        return nextDay;
    }

    /// <summary>
    /// Checks if the time component of the given DateTime falls within the specified time window (e.g. start &lt;= dateTime &lt; end).
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="startInclusive"></param>
    /// <param name="endExclusive"></param>
    /// <returns></returns>
    /// <remarks>The time window can span across midnight. For example, a time window from 22:00 to 02:00 will include times from 22:00 to 23:59 and from 00:00 to 02:00.</remarks>
    public static bool IsWithinTimeWindow(this DateTime dateTime, TimeOnly startInclusive, TimeOnly endExclusive)
    {
        var time = TimeOnly.FromDateTime(dateTime);
        return startInclusive <= endExclusive
            ? time >= startInclusive && time < endExclusive
            : time >= startInclusive || time < endExclusive;
    }

    /// <summary>
    /// Checks if the day of week component of the given DateTime falls on any of the specified days of week.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="dows"></param>
    /// <returns></returns>
    public static bool IsOnDayOfWeek(this DateTime dateTime, params DayOfWeek[] dows)
    {
        return dows.Contains(dateTime.DayOfWeek);
    }

    /// <summary>
    /// Checks if the day of week component of the given DateTime falls on any of the specified days of week.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="dows"></param>
    /// <returns></returns>
    public static bool IsOnDayOfWeek(this DateTime dateTime, IEnumerable<DayOfWeek> dows)
    {
        return dows.Contains(dateTime.DayOfWeek);
    }

    /// <summary>
    /// Checks if the day of week component of the given DateTime falls on any of the specified days of week (case-insensitive, supports both full and abbreviated day of week names).
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="dows"></param>
    /// <returns></returns>
    public static bool IsOnDayOfWeek(this DateTime dateTime, params string[] dows)
    {
        var dowStr1 = dateTime.DayOfWeek.ToString().ToUpper();
        var dowStr2 = dowStr1.Substring(0, 3);
        return dows.Where(d => d.Equals(dowStr1, StringComparison.OrdinalIgnoreCase) || d.Equals(dowStr2, StringComparison.OrdinalIgnoreCase)).Any();
    }

    /// <summary>
    /// Checks if the day of week component of the given DateTime falls on any of the specified days of week (case-insensitive, supports both full and abbreviated day of week names).
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="dows"></param>
    /// <returns></returns>
    public static bool IsOnDayOfWeek(this DateTime dateTime, IEnumerable<string> dows)
    {
        var dowStr1 = dateTime.DayOfWeek.ToString().ToUpper();
        var dowStr2 = dowStr1.Substring(0, 3);
        return dows.Where(d => d.Equals(dowStr1, StringComparison.OrdinalIgnoreCase) || d.Equals(dowStr2, StringComparison.OrdinalIgnoreCase)).Any();
    }
}
