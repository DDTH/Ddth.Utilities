using Ddth.Utilities.Tempus;

namespace Ddth.Utilities.Tests.Tempus;

public class DateTimeOffsetExtensionsTest
{
    /*----------------------------------------------------------------------*/
    /* StartOfDay                                                           */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestStartOfDay_ZeroesTimeComponent()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 14, 30, 45, 123, TimeSpan.FromHours(10));
        var result = dt.StartOfDay();
        Assert.Equal(0, result.Hour);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Second);
        Assert.Equal(0, result.Millisecond);
    }

    [Fact]
    public void TestStartOfDay_PreservesDateAndOffset()
    {
        var offset = TimeSpan.FromHours(5.5);
        var dt = new DateTimeOffset(2026, 12, 31, 23, 59, 59, offset);
        var result = dt.StartOfDay();
        Assert.Equal(2026, result.Year);
        Assert.Equal(12, result.Month);
        Assert.Equal(31, result.Day);
        Assert.Equal(offset, result.Offset);
    }

    [Fact]
    public void TestStartOfDay_AlreadyMidnight()
    {
        var dt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var result = dt.StartOfDay();
        Assert.Equal(dt, result);
    }

    /*----------------------------------------------------------------------*/
    /* StartOfMonth                                                         */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestStartOfMonth_ResetsToFirstDayAndZeroesTime()
    {
        var dt = new DateTimeOffset(2026, 7, 18, 14, 30, 45, 123, TimeSpan.FromHours(10));
        var result = dt.StartOfMonth();
        Assert.Equal(2026, result.Year);
        Assert.Equal(7, result.Month);
        Assert.Equal(1, result.Day);
        Assert.Equal(0, result.Hour);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Second);
        Assert.Equal(0, result.Millisecond);
    }

    [Fact]
    public void TestStartOfMonth_PreservesYearMonthAndOffset()
    {
        var offset = TimeSpan.FromHours(5.5);
        var dt = new DateTimeOffset(2026, 12, 31, 23, 59, 59, offset);
        var result = dt.StartOfMonth();
        Assert.Equal(2026, result.Year);
        Assert.Equal(12, result.Month);
        Assert.Equal(1, result.Day);
        Assert.Equal(offset, result.Offset);
    }

    [Fact]
    public void TestStartOfMonth_AlreadyFirstDayMidnight()
    {
        var dt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var result = dt.StartOfMonth();
        Assert.Equal(dt, result);
    }

    /*----------------------------------------------------------------------*/
    /* PrevWeekDay                                                          */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestPrevWeekDay_FromWeekday()
    {
        // Wednesday 2026-04-15
        var dt = new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero);
        var result = dt.PrevWeekDay();
        Assert.Equal(DayOfWeek.Tuesday, result.DayOfWeek);
    }

    [Fact]
    public void TestPrevWeekDay_FromMonday_SkipsWeekend()
    {
        // Monday 2026-04-13
        var dt = new DateTimeOffset(2026, 4, 13, 12, 0, 0, TimeSpan.Zero);
        Assert.Equal(DayOfWeek.Monday, dt.DayOfWeek);
        var result = dt.PrevWeekDay();
        Assert.Equal(DayOfWeek.Friday, result.DayOfWeek);
        Assert.Equal(10, result.Day);
    }

    [Fact]
    public void TestPrevWeekDay_FromSunday()
    {
        // Sunday 2026-04-12
        var dt = new DateTimeOffset(2026, 4, 12, 12, 0, 0, TimeSpan.Zero);
        Assert.Equal(DayOfWeek.Sunday, dt.DayOfWeek);
        var result = dt.PrevWeekDay();
        Assert.Equal(DayOfWeek.Friday, result.DayOfWeek);
    }

    /*----------------------------------------------------------------------*/
    /* NextWeekDay                                                          */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestNextWeekDay_FromWeekday()
    {
        var dt = new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero);
        var result = dt.NextWeekDay();
        Assert.Equal(DayOfWeek.Thursday, result.DayOfWeek);
    }

    [Fact]
    public void TestNextWeekDay_FromFriday_SkipsWeekend()
    {
        // Friday 2026-04-17
        var dt = new DateTimeOffset(2026, 4, 17, 12, 0, 0, TimeSpan.Zero);
        Assert.Equal(DayOfWeek.Friday, dt.DayOfWeek);
        var result = dt.NextWeekDay();
        Assert.Equal(DayOfWeek.Monday, result.DayOfWeek);
        Assert.Equal(20, result.Day);
    }

    [Fact]
    public void TestNextWeekDay_FromSaturday()
    {
        // Saturday 2026-04-18
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.Equal(DayOfWeek.Saturday, dt.DayOfWeek);
        var result = dt.NextWeekDay();
        Assert.Equal(DayOfWeek.Monday, result.DayOfWeek);
    }

    /*----------------------------------------------------------------------*/
    /* IsWithinTimeWindow                                                     */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestIsWithinTimeWindow_InsideNormalWindow()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 10, 0, 0, TimeSpan.Zero);
        Assert.True(dt.IsWithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_OutsideNormalWindow()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 18, 0, 0, TimeSpan.Zero);
        Assert.False(dt.IsWithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_BeforeStart()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 7, 0, 0, TimeSpan.Zero);
        Assert.False(dt.IsWithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_AtStartBoundary_Inclusive()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 9, 0, 0, TimeSpan.Zero);
        Assert.True(dt.IsWithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_AtEndBoundary_Exclusive()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 17, 0, 0, TimeSpan.Zero);
        Assert.False(dt.IsWithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_AcrossMidnight_BeforeMidnight()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 23, 0, 0, TimeSpan.Zero);
        Assert.True(dt.IsWithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_AcrossMidnight_AfterMidnight()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 1, 0, 0, TimeSpan.Zero);
        Assert.True(dt.IsWithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_AcrossMidnight_Outside()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 15, 0, 0, TimeSpan.Zero);
        Assert.False(dt.IsWithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_StartEqualsEnd_AlwaysFalse()
    {
        var exact = new DateTimeOffset(2026, 4, 18, 9, 0, 0, TimeSpan.Zero);
        Assert.False(exact.IsWithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(9, 0)));

        var other = new DateTimeOffset(2026, 4, 18, 15, 0, 0, TimeSpan.Zero);
        Assert.False(other.IsWithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(9, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_AcrossMidnight_AtStartBoundary_Inclusive()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 22, 0, 0, TimeSpan.Zero);
        Assert.True(dt.IsWithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_AcrossMidnight_AtEndBoundary_Exclusive()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 2, 0, 0, TimeSpan.Zero);
        Assert.False(dt.IsWithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    /*----------------------------------------------------------------------*/
    /* IsOnDayOfWeek                                                        */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestIsOnDayOfWeek_DayOfWeekParams_Match()
    {
        // Saturday
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.True(dt.IsOnDayOfWeek(DayOfWeek.Saturday, DayOfWeek.Sunday));
    }

    [Fact]
    public void TestIsOnDayOfWeek_DayOfWeekParams_NoMatch()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.False(dt.IsOnDayOfWeek(DayOfWeek.Monday, DayOfWeek.Tuesday));
    }

    [Fact]
    public void TestIsOnDayOfWeek_DayOfWeekEnumerable()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        IEnumerable<DayOfWeek> dows = new List<DayOfWeek> { DayOfWeek.Saturday };
        Assert.True(dt.IsOnDayOfWeek(dows));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringParams_FullName()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.True(dt.IsOnDayOfWeek("Saturday"));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringParams_Abbreviated()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.True(dt.IsOnDayOfWeek("Sat"));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringParams_CaseInsensitive()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.True(dt.IsOnDayOfWeek("saturday"));
        Assert.True(dt.IsOnDayOfWeek("SATURDAY"));
        Assert.True(dt.IsOnDayOfWeek("sat"));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringParams_NoMatch()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.False(dt.IsOnDayOfWeek("Monday", "Tuesday"));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringParams_MatchByAbbreviatedAfterNonMatch()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.True(dt.IsOnDayOfWeek("Monday", "Sat"));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringParams_MatchByFullNameAfterNonMatch()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.True(dt.IsOnDayOfWeek("Mon", "Saturday"));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringEnumerable()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        IEnumerable<string> dows = new List<string> { "Sat", "Sun" };
        Assert.True(dt.IsOnDayOfWeek(dows));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringEnumerable_NoMatch()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        IEnumerable<string> dows = new List<string> { "Mon", "Tue" };
        Assert.False(dt.IsOnDayOfWeek(dows));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringEnumerable_MatchByAbbreviatedAfterNonMatch()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        IEnumerable<string> dows = new List<string> { "Monday", "Sat" };
        Assert.True(dt.IsOnDayOfWeek(dows));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringEnumerable_MatchByFullNameAfterNonMatch()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        IEnumerable<string> dows = new List<string> { "Mon", "Saturday" };
        Assert.True(dt.IsOnDayOfWeek(dows));
    }

    /*----------------------------------------------------------------------*/
    /* ToTimeZoneSilently                                                   */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestToTimeZoneSilently_ValidTimeZone()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero); // UTC noon
        var result = dt.ToTimeZoneSilently("Australia/Sydney");
        Assert.NotNull(result);
        Assert.NotEqual(TimeSpan.Zero, result.Value.Offset);
    }

    [Fact]
    public void TestToTimeZoneSilently_PreservesInstant()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        var result = dt.ToTimeZoneSilently("Australia/Sydney");
        Assert.NotNull(result);
        Assert.Equal(dt.UtcDateTime, result.Value.UtcDateTime);
    }

    [Fact]
    public void TestToTimeZoneSilently_InvalidTimeZone_ReturnsNull()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        var result = dt.ToTimeZoneSilently("Invalid/TimeZone");
        Assert.Null(result);
    }

    /*----------------------------------------------------------------------*/
    /* AsTimeZoneSilently                                                   */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestAsTimeZoneSilently_ValidTimeZone_KeepsDateTime()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        var result = dt.AsTimeZoneSilently("Australia/Sydney");
        Assert.NotNull(result);
        // The date and time components should remain the same
        Assert.Equal(dt.Year, result.Value.Year);
        Assert.Equal(dt.Month, result.Value.Month);
        Assert.Equal(dt.Day, result.Value.Day);
        Assert.Equal(dt.Hour, result.Value.Hour);
        Assert.Equal(dt.Minute, result.Value.Minute);
    }

    [Fact]
    public void TestAsTimeZoneSilently_ValidTimeZone_ChangesOffset()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        var result = dt.AsTimeZoneSilently("Australia/Sydney");
        Assert.NotNull(result);
        Assert.NotEqual(TimeSpan.Zero, result.Value.Offset);
    }

    [Fact]
    public void TestAsTimeZoneSilently_InvalidTimeZone_ReturnsNull()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        var result = dt.AsTimeZoneSilently("Invalid/TimeZone");
        Assert.Null(result);
    }
}
