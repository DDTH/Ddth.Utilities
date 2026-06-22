using Ddth.Utilities.Tempus;

namespace Ddth.Utilities.Tests.Tempus;

public class DateTimeExtensionsTest
{
    /*----------------------------------------------------------------------*/
    /* StartOfDay                                                           */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestStartOfDay_ZeroesTimeComponent()
    {
#if NET6_0
        var dt = new DateTime(2026, 4, 18, 14, 30, 45, 123, DateTimeKind.Local);
#else
        var dt = new DateTime(2026, 4, 18, 14, 30, 45, 123, 456, DateTimeKind.Local);
#endif
        var result = dt.StartOfDay();
#if NET6_0
        Assert.Equal(new DateTime(2026, 4, 18, 0, 0, 0, 0, DateTimeKind.Local), result);
#else
        Assert.Equal(new DateTime(2026, 4, 18, 0, 0, 0, 0, 0, DateTimeKind.Local), result);
#endif
    }

    [Fact]
    public void TestStartOfDay_PreservesDateAndKind()
    {
        var dt = new DateTime(2026, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc);
        var result = dt.StartOfDay();
        Assert.Equal(2026, result.Year);
        Assert.Equal(12, result.Month);
        Assert.Equal(31, result.Day);
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

    [Fact]
    public void TestStartOfDay_AlreadyMidnight()
    {
#if NET6_0
        var dt = new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
#else
        var dt = new DateTime(2026, 1, 1, 0, 0, 0, 0, 0, DateTimeKind.Utc);
#endif
        var result = dt.StartOfDay();
        Assert.Equal(dt, result);
    }

    /*----------------------------------------------------------------------*/
    /* StartOfMonth                                                         */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestStartOfMonth_ResetsToFirstDayAndZeroesTime()
    {
#if NET6_0
        var dt = new DateTime(2026, 7, 18, 14, 30, 45, 123, DateTimeKind.Local);
#else
        var dt = new DateTime(2026, 7, 18, 14, 30, 45, 123, 456, DateTimeKind.Local);
#endif
        var result = dt.StartOfMonth();
#if NET6_0
        Assert.Equal(new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Local), result);
#else
        Assert.Equal(new DateTime(2026, 7, 1, 0, 0, 0, 0, 0, DateTimeKind.Local), result);
#endif
    }

    [Fact]
    public void TestStartOfMonth_PreservesYearMonthAndKind()
    {
        var dt = new DateTime(2026, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc);
        var result = dt.StartOfMonth();
        Assert.Equal(2026, result.Year);
        Assert.Equal(12, result.Month);
        Assert.Equal(1, result.Day);
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

    [Fact]
    public void TestStartOfMonth_AlreadyFirstDayMidnight()
    {
#if NET6_0
        var dt = new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
#else
        var dt = new DateTime(2026, 1, 1, 0, 0, 0, 0, 0, DateTimeKind.Utc);
#endif
        var result = dt.StartOfMonth();
        Assert.Equal(dt, result);
    }

    /*----------------------------------------------------------------------*/
    /* StartOfQuarter                                                       */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestStartOfQuarter_Q1()
    {
        var dt = new DateTime(2026, 2, 15, 10, 30, 0, DateTimeKind.Utc);
        var result = dt.StartOfQuarter();
        Assert.Equal(2026, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(1, result.Day);
        Assert.Equal(0, result.Hour);
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

    [Fact]
    public void TestStartOfQuarter_Q2()
    {
        var dt = new DateTime(2026, 6, 30, 23, 59, 59, DateTimeKind.Local);
        var result = dt.StartOfQuarter();
        Assert.Equal(4, result.Month);
        Assert.Equal(1, result.Day);
        Assert.Equal(0, result.Hour);
    }

    [Fact]
    public void TestStartOfQuarter_Q3()
    {
        var dt = new DateTime(2026, 9, 1);
        var result = dt.StartOfQuarter();
        Assert.Equal(7, result.Month);
        Assert.Equal(1, result.Day);
    }

    [Fact]
    public void TestStartOfQuarter_Q4()
    {
        var dt = new DateTime(2026, 12, 31, 23, 59, 59, DateTimeKind.Utc);
        var result = dt.StartOfQuarter();
        Assert.Equal(10, result.Month);
        Assert.Equal(1, result.Day);
    }

    [Fact]
    public void TestStartOfQuarter_AlreadyStartOfQuarter()
    {
#if NET6_0
        var dt = new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc);
#else
        var dt = new DateTime(2026, 7, 1, 0, 0, 0, 0, 0, DateTimeKind.Utc);
#endif
        var result = dt.StartOfQuarter();
        Assert.Equal(dt, result);
    }

    /*----------------------------------------------------------------------*/
    /* StartOfCalendarYear                                                          */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestStartOfCalendarYear_MidYear()
    {
        var dt = new DateTime(2026, 7, 15, 14, 30, 0, DateTimeKind.Local);
        var result = dt.StartOfCalendarYear();
        Assert.Equal(2026, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(1, result.Day);
        Assert.Equal(0, result.Hour);
        Assert.Equal(DateTimeKind.Local, result.Kind);
    }

    [Fact]
    public void TestStartOfCalendarYear_EndOfYear()
    {
        var dt = new DateTime(2026, 12, 31, 23, 59, 59, DateTimeKind.Utc);
        var result = dt.StartOfCalendarYear();
        Assert.Equal(2026, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(1, result.Day);
        Assert.Equal(0, result.Hour);
    }

    [Fact]
    public void TestStartOfCalendarYear_AlreadyStartOfCalendarYear()
    {
#if NET6_0
        var dt = new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
#else
        var dt = new DateTime(2026, 1, 1, 0, 0, 0, 0, 0, DateTimeKind.Utc);
#endif
        var result = dt.StartOfCalendarYear();
        Assert.Equal(dt, result);
    }

    /*----------------------------------------------------------------------*/
    /* StartOfWeek                                                          */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestStartOfWeek_DefaultMonday_FromWednesday()
    {
        // Wednesday 2026-06-17 -> Monday 2026-06-15
        var dt = new DateTime(2026, 6, 17, 10, 30, 0, DateTimeKind.Local);
        var result = dt.StartOfWeek();
        Assert.Equal(DayOfWeek.Monday, result.DayOfWeek);
        Assert.Equal(new DateTime(2026, 6, 15), result.Date);
        Assert.Equal(0, result.Hour);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Second);
    }

    [Fact]
    public void TestStartOfWeek_DefaultMonday_FromMonday()
    {
        // Monday 2026-06-15 -> stays Monday 2026-06-15
        var dt = new DateTime(2026, 6, 15, 14, 0, 0, DateTimeKind.Utc);
        var result = dt.StartOfWeek();
        Assert.Equal(new DateTime(2026, 6, 15), result.Date);
        Assert.Equal(0, result.Hour);
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

    [Fact]
    public void TestStartOfWeek_DefaultMonday_FromSunday()
    {
        // Sunday 2026-06-21 -> Monday 2026-06-15
        var dt = new DateTime(2026, 6, 21);
        Assert.Equal(DayOfWeek.Sunday, dt.DayOfWeek);
        var result = dt.StartOfWeek();
        Assert.Equal(DayOfWeek.Monday, result.DayOfWeek);
        Assert.Equal(new DateTime(2026, 6, 15), result.Date);
    }

    [Fact]
    public void TestStartOfWeek_Sunday_FromWednesday()
    {
        // Wednesday 2026-06-17 -> Sunday 2026-06-14
        var dt = new DateTime(2026, 6, 17, 10, 30, 0, DateTimeKind.Local);
        var result = dt.StartOfWeek(DayOfWeek.Sunday);
        Assert.Equal(DayOfWeek.Sunday, result.DayOfWeek);
        Assert.Equal(new DateTime(2026, 6, 14), result.Date);
    }

    [Fact]
    public void TestStartOfWeek_Sunday_FromSunday()
    {
        // Sunday 2026-06-14 -> stays Sunday 2026-06-14
        var dt = new DateTime(2026, 6, 14, 23, 59, 59);
        Assert.Equal(DayOfWeek.Sunday, dt.DayOfWeek);
        var result = dt.StartOfWeek(DayOfWeek.Sunday);
        Assert.Equal(new DateTime(2026, 6, 14), result.Date);
        Assert.Equal(0, result.Hour);
    }

    [Fact]
    public void TestStartOfWeek_Saturday_FromSunday()
    {
        // Sunday 2026-06-21 -> Saturday 2026-06-20
        var dt = new DateTime(2026, 6, 21);
        var result = dt.StartOfWeek(DayOfWeek.Saturday);
        Assert.Equal(DayOfWeek.Saturday, result.DayOfWeek);
        Assert.Equal(new DateTime(2026, 6, 20), result.Date);
    }

    /*----------------------------------------------------------------------*/
    /* PrevWeekDay                                                          */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestPrevWeekDay_FromWeekday()
    {
        // Wednesday 2026-04-15 -> Tuesday 2026-04-14
        var dt = new DateTime(2026, 4, 15);
        var result = dt.PrevWeekDay();
        Assert.Equal(DayOfWeek.Tuesday, result.DayOfWeek);
        Assert.Equal(new DateTime(2026, 4, 14), result.Date);
    }

    [Fact]
    public void TestPrevWeekDay_FromMonday_SkipsWeekend()
    {
        // Monday 2026-04-13 -> Friday 2026-04-10
        var dt = new DateTime(2026, 4, 13);
        Assert.Equal(DayOfWeek.Monday, dt.DayOfWeek);
        var result = dt.PrevWeekDay();
        Assert.Equal(DayOfWeek.Friday, result.DayOfWeek);
        Assert.Equal(new DateTime(2026, 4, 10), result.Date);
    }

    [Fact]
    public void TestPrevWeekDay_FromSaturday()
    {
        // Saturday 2026-04-11 -> Friday 2026-04-10
        var dt = new DateTime(2026, 4, 11);
        Assert.Equal(DayOfWeek.Saturday, dt.DayOfWeek);
        var result = dt.PrevWeekDay();
        Assert.Equal(DayOfWeek.Friday, result.DayOfWeek);
    }

    [Fact]
    public void TestPrevWeekDay_FromSunday()
    {
        // Sunday 2026-04-12 -> Friday 2026-04-10
        var dt = new DateTime(2026, 4, 12);
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
        // Wednesday 2026-04-15 -> Thursday 2026-04-16
        var dt = new DateTime(2026, 4, 15);
        var result = dt.NextWeekDay();
        Assert.Equal(DayOfWeek.Thursday, result.DayOfWeek);
        Assert.Equal(new DateTime(2026, 4, 16), result.Date);
    }

    [Fact]
    public void TestNextWeekDay_FromFriday_SkipsWeekend()
    {
        // Friday 2026-04-17 -> Monday 2026-04-20
        var dt = new DateTime(2026, 4, 17);
        Assert.Equal(DayOfWeek.Friday, dt.DayOfWeek);
        var result = dt.NextWeekDay();
        Assert.Equal(DayOfWeek.Monday, result.DayOfWeek);
        Assert.Equal(new DateTime(2026, 4, 20), result.Date);
    }

    [Fact]
    public void TestNextWeekDay_FromSaturday()
    {
        // Saturday 2026-04-18 -> Monday 2026-04-20
        var dt = new DateTime(2026, 4, 18);
        Assert.Equal(DayOfWeek.Saturday, dt.DayOfWeek);
        var result = dt.NextWeekDay();
        Assert.Equal(DayOfWeek.Monday, result.DayOfWeek);
    }

    [Fact]
    public void TestNextWeekDay_FromSunday()
    {
        // Sunday 2026-04-19 -> Monday 2026-04-20
        var dt = new DateTime(2026, 4, 19);
        Assert.Equal(DayOfWeek.Sunday, dt.DayOfWeek);
        var result = dt.NextWeekDay();
        Assert.Equal(DayOfWeek.Monday, result.DayOfWeek);
    }

    /*----------------------------------------------------------------------*/
    /* IsWithinTimeWindow                                                     */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestIsWithinTimeWindow_InsideNormalWindow()
    {
        var dt = new DateTime(2026, 4, 18, 10, 0, 0);
        Assert.True(dt.IsWithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_OutsideNormalWindow()
    {
        var dt = new DateTime(2026, 4, 18, 18, 0, 0);
        Assert.False(dt.IsWithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_BeforeStart()
    {
        // time < start: short-circuits the && on the normal-window branch
        var dt = new DateTime(2026, 4, 18, 7, 0, 0);
        Assert.False(dt.IsWithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_AtStartBoundary_Inclusive()
    {
        var dt = new DateTime(2026, 4, 18, 9, 0, 0);
        Assert.True(dt.IsWithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_AtEndBoundary_Exclusive()
    {
        var dt = new DateTime(2026, 4, 18, 17, 0, 0);
        Assert.False(dt.IsWithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_AcrossMidnight_BeforeMidnight()
    {
        var dt = new DateTime(2026, 4, 18, 23, 0, 0);
        Assert.True(dt.IsWithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_AcrossMidnight_AfterMidnight()
    {
        var dt = new DateTime(2026, 4, 18, 1, 0, 0);
        Assert.True(dt.IsWithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_AcrossMidnight_Outside()
    {
        var dt = new DateTime(2026, 4, 18, 15, 0, 0);
        Assert.False(dt.IsWithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_StartEqualsEnd_AlwaysFalse()
    {
        // When start == end the window is empty: time >= start && time < end is always false
        var exact = new DateTime(2026, 4, 18, 9, 0, 0);
        Assert.False(exact.IsWithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(9, 0)));

        var other = new DateTime(2026, 4, 18, 15, 0, 0);
        Assert.False(other.IsWithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(9, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_AcrossMidnight_AtStartBoundary_Inclusive()
    {
        var dt = new DateTime(2026, 4, 18, 22, 0, 0);
        Assert.True(dt.IsWithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [Fact]
    public void TestIsWithinTimeWindow_AcrossMidnight_AtEndBoundary_Exclusive()
    {
        var dt = new DateTime(2026, 4, 18, 2, 0, 0);
        Assert.False(dt.IsWithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    /*----------------------------------------------------------------------*/
    /* IsOnDayOfWeek                                                        */
    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestIsOnDayOfWeek_DayOfWeekParams_Match()
    {
        // Saturday
        var dt = new DateTime(2026, 4, 18);
        Assert.True(dt.IsOnDayOfWeek(DayOfWeek.Saturday, DayOfWeek.Sunday));
    }

    [Fact]
    public void TestIsOnDayOfWeek_DayOfWeekParams_NoMatch()
    {
        var dt = new DateTime(2026, 4, 18);
        Assert.False(dt.IsOnDayOfWeek(DayOfWeek.Monday, DayOfWeek.Tuesday));
    }

    [Fact]
    public void TestIsOnDayOfWeek_DayOfWeekEnumerable()
    {
        var dt = new DateTime(2026, 4, 18);
        IEnumerable<DayOfWeek> dows = new List<DayOfWeek> { DayOfWeek.Saturday };
        Assert.True(dt.IsOnDayOfWeek(dows));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringParams_FullName()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        Assert.True(dt.IsOnDayOfWeek("Saturday"));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringParams_Abbreviated()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        Assert.True(dt.IsOnDayOfWeek("Sat"));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringParams_CaseInsensitive()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        Assert.True(dt.IsOnDayOfWeek("saturday"));
        Assert.True(dt.IsOnDayOfWeek("SATURDAY"));
        Assert.True(dt.IsOnDayOfWeek("sat"));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringParams_NoMatch()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        Assert.False(dt.IsOnDayOfWeek("Monday", "Tuesday"));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringParams_MatchByAbbreviatedAfterNonMatch()
    {
        // "Monday" fails both sides of ||, then "Sat" fails full-name but matches abbreviated
        var dt = new DateTime(2026, 4, 18); // Saturday
        Assert.True(dt.IsOnDayOfWeek("Monday", "Sat"));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringParams_MatchByFullNameAfterNonMatch()
    {
        // "Mon" fails both sides of ||, then "Saturday" matches full-name (short-circuits ||)
        var dt = new DateTime(2026, 4, 18); // Saturday
        Assert.True(dt.IsOnDayOfWeek("Mon", "Saturday"));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringEnumerable()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        IEnumerable<string> dows = new List<string> { "Sat", "Sun" };
        Assert.True(dt.IsOnDayOfWeek(dows));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringEnumerable_NoMatch()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        IEnumerable<string> dows = new List<string> { "Mon", "Tue" };
        Assert.False(dt.IsOnDayOfWeek(dows));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringEnumerable_MatchByAbbreviatedAfterNonMatch()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        IEnumerable<string> dows = new List<string> { "Monday", "Sat" };
        Assert.True(dt.IsOnDayOfWeek(dows));
    }

    [Fact]
    public void TestIsOnDayOfWeek_StringEnumerable_MatchByFullNameAfterNonMatch()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        IEnumerable<string> dows = new List<string> { "Mon", "Saturday" };
        Assert.True(dt.IsOnDayOfWeek(dows));
    }
}
