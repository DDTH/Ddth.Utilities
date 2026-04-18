using Ddth.Utilities.Tempus;

namespace Ddth.Utilities.Tests.Tempus;

[TestClass]
public class DateTimeExtensionsTest
{
    /*----------------------------------------------------------------------*/
    /* StartOfDay                                                           */
    /*----------------------------------------------------------------------*/

    [TestMethod]
    public void TestStartOfDay_ZeroesTimeComponent()
    {
#if NET6_0
        var dt = new DateTime(2026, 4, 18, 14, 30, 45, 123, DateTimeKind.Local);
#else
        var dt = new DateTime(2026, 4, 18, 14, 30, 45, 123, 456, DateTimeKind.Local);
#endif
        var result = dt.StartOfDay();
#if NET6_0
        Assert.AreEqual(new DateTime(2026, 4, 18, 0, 0, 0, 0, DateTimeKind.Local), result);
#else
        Assert.AreEqual(new DateTime(2026, 4, 18, 0, 0, 0, 0, 0, DateTimeKind.Local), result);
#endif
    }

    [TestMethod]
    public void TestStartOfDay_PreservesDateAndKind()
    {
        var dt = new DateTime(2026, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc);
        var result = dt.StartOfDay();
        Assert.AreEqual(2026, result.Year);
        Assert.AreEqual(12, result.Month);
        Assert.AreEqual(31, result.Day);
        Assert.AreEqual(DateTimeKind.Utc, result.Kind);
    }

    [TestMethod]
    public void TestStartOfDay_AlreadyMidnight()
    {
#if NET6_0
        var dt = new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
#else
        var dt = new DateTime(2026, 1, 1, 0, 0, 0, 0, 0, DateTimeKind.Utc);
#endif
        var result = dt.StartOfDay();
        Assert.AreEqual(dt, result);
    }

    /*----------------------------------------------------------------------*/
    /* PrevWeekDay                                                          */
    /*----------------------------------------------------------------------*/

    [TestMethod]
    public void TestPrevWeekDay_FromWeekday()
    {
        // Wednesday 2026-04-15 -> Tuesday 2026-04-14
        var dt = new DateTime(2026, 4, 15);
        var result = dt.PrevWeekDay();
        Assert.AreEqual(DayOfWeek.Tuesday, result.DayOfWeek);
        Assert.AreEqual(new DateTime(2026, 4, 14), result.Date);
    }

    [TestMethod]
    public void TestPrevWeekDay_FromMonday_SkipsWeekend()
    {
        // Monday 2026-04-13 -> Friday 2026-04-10
        var dt = new DateTime(2026, 4, 13);
        Assert.AreEqual(DayOfWeek.Monday, dt.DayOfWeek);
        var result = dt.PrevWeekDay();
        Assert.AreEqual(DayOfWeek.Friday, result.DayOfWeek);
        Assert.AreEqual(new DateTime(2026, 4, 10), result.Date);
    }

    [TestMethod]
    public void TestPrevWeekDay_FromSaturday()
    {
        // Saturday 2026-04-11 -> Friday 2026-04-10
        var dt = new DateTime(2026, 4, 11);
        Assert.AreEqual(DayOfWeek.Saturday, dt.DayOfWeek);
        var result = dt.PrevWeekDay();
        Assert.AreEqual(DayOfWeek.Friday, result.DayOfWeek);
    }

    [TestMethod]
    public void TestPrevWeekDay_FromSunday()
    {
        // Sunday 2026-04-12 -> Friday 2026-04-10
        var dt = new DateTime(2026, 4, 12);
        Assert.AreEqual(DayOfWeek.Sunday, dt.DayOfWeek);
        var result = dt.PrevWeekDay();
        Assert.AreEqual(DayOfWeek.Friday, result.DayOfWeek);
    }

    /*----------------------------------------------------------------------*/
    /* NextWeekDay                                                          */
    /*----------------------------------------------------------------------*/

    [TestMethod]
    public void TestNextWeekDay_FromWeekday()
    {
        // Wednesday 2026-04-15 -> Thursday 2026-04-16
        var dt = new DateTime(2026, 4, 15);
        var result = dt.NextWeekDay();
        Assert.AreEqual(DayOfWeek.Thursday, result.DayOfWeek);
        Assert.AreEqual(new DateTime(2026, 4, 16), result.Date);
    }

    [TestMethod]
    public void TestNextWeekDay_FromFriday_SkipsWeekend()
    {
        // Friday 2026-04-17 -> Monday 2026-04-20
        var dt = new DateTime(2026, 4, 17);
        Assert.AreEqual(DayOfWeek.Friday, dt.DayOfWeek);
        var result = dt.NextWeekDay();
        Assert.AreEqual(DayOfWeek.Monday, result.DayOfWeek);
        Assert.AreEqual(new DateTime(2026, 4, 20), result.Date);
    }

    [TestMethod]
    public void TestNextWeekDay_FromSaturday()
    {
        // Saturday 2026-04-18 -> Monday 2026-04-20
        var dt = new DateTime(2026, 4, 18);
        Assert.AreEqual(DayOfWeek.Saturday, dt.DayOfWeek);
        var result = dt.NextWeekDay();
        Assert.AreEqual(DayOfWeek.Monday, result.DayOfWeek);
    }

    [TestMethod]
    public void TestNextWeekDay_FromSunday()
    {
        // Sunday 2026-04-19 -> Monday 2026-04-20
        var dt = new DateTime(2026, 4, 19);
        Assert.AreEqual(DayOfWeek.Sunday, dt.DayOfWeek);
        var result = dt.NextWeekDay();
        Assert.AreEqual(DayOfWeek.Monday, result.DayOfWeek);
    }

    /*----------------------------------------------------------------------*/
    /* WithinTimeWindow                                                     */
    /*----------------------------------------------------------------------*/

    [TestMethod]
    public void TestWithinTimeWindow_InsideNormalWindow()
    {
        var dt = new DateTime(2026, 4, 18, 10, 0, 0);
        Assert.IsTrue(dt.WithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_OutsideNormalWindow()
    {
        var dt = new DateTime(2026, 4, 18, 18, 0, 0);
        Assert.IsFalse(dt.WithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_BeforeStart()
    {
        // time < start: short-circuits the && on the normal-window branch
        var dt = new DateTime(2026, 4, 18, 7, 0, 0);
        Assert.IsFalse(dt.WithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_AtStartBoundary_Inclusive()
    {
        var dt = new DateTime(2026, 4, 18, 9, 0, 0);
        Assert.IsTrue(dt.WithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_AtEndBoundary_Exclusive()
    {
        var dt = new DateTime(2026, 4, 18, 17, 0, 0);
        Assert.IsFalse(dt.WithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_AcrossMidnight_BeforeMidnight()
    {
        var dt = new DateTime(2026, 4, 18, 23, 0, 0);
        Assert.IsTrue(dt.WithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_AcrossMidnight_AfterMidnight()
    {
        var dt = new DateTime(2026, 4, 18, 1, 0, 0);
        Assert.IsTrue(dt.WithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_AcrossMidnight_Outside()
    {
        var dt = new DateTime(2026, 4, 18, 15, 0, 0);
        Assert.IsFalse(dt.WithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_StartEqualsEnd_AlwaysFalse()
    {
        // When start == end the window is empty: time >= start && time < end is always false
        var exact = new DateTime(2026, 4, 18, 9, 0, 0);
        Assert.IsFalse(exact.WithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(9, 0)));

        var other = new DateTime(2026, 4, 18, 15, 0, 0);
        Assert.IsFalse(other.WithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(9, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_AcrossMidnight_AtStartBoundary_Inclusive()
    {
        var dt = new DateTime(2026, 4, 18, 22, 0, 0);
        Assert.IsTrue(dt.WithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_AcrossMidnight_AtEndBoundary_Exclusive()
    {
        var dt = new DateTime(2026, 4, 18, 2, 0, 0);
        Assert.IsFalse(dt.WithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    /*----------------------------------------------------------------------*/
    /* WithinDowList                                                        */
    /*----------------------------------------------------------------------*/

    [TestMethod]
    public void TestWithinDowList_DayOfWeekParams_Match()
    {
        // Saturday
        var dt = new DateTime(2026, 4, 18);
        Assert.IsTrue(dt.WithinDowList(DayOfWeek.Saturday, DayOfWeek.Sunday));
    }

    [TestMethod]
    public void TestWithinDowList_DayOfWeekParams_NoMatch()
    {
        var dt = new DateTime(2026, 4, 18);
        Assert.IsFalse(dt.WithinDowList(DayOfWeek.Monday, DayOfWeek.Tuesday));
    }

    [TestMethod]
    public void TestWithinDowList_DayOfWeekEnumerable()
    {
        var dt = new DateTime(2026, 4, 18);
        IEnumerable<DayOfWeek> dows = new List<DayOfWeek> { DayOfWeek.Saturday };
        Assert.IsTrue(dt.WithinDowList(dows));
    }

    [TestMethod]
    public void TestWithinDowList_StringParams_FullName()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        Assert.IsTrue(dt.WithinDowList("Saturday"));
    }

    [TestMethod]
    public void TestWithinDowList_StringParams_Abbreviated()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        Assert.IsTrue(dt.WithinDowList("Sat"));
    }

    [TestMethod]
    public void TestWithinDowList_StringParams_CaseInsensitive()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        Assert.IsTrue(dt.WithinDowList("saturday"));
        Assert.IsTrue(dt.WithinDowList("SATURDAY"));
        Assert.IsTrue(dt.WithinDowList("sat"));
    }

    [TestMethod]
    public void TestWithinDowList_StringParams_NoMatch()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        Assert.IsFalse(dt.WithinDowList("Monday", "Tuesday"));
    }

    [TestMethod]
    public void TestWithinDowList_StringParams_MatchByAbbreviatedAfterNonMatch()
    {
        // "Monday" fails both sides of ||, then "Sat" fails full-name but matches abbreviated
        var dt = new DateTime(2026, 4, 18); // Saturday
        Assert.IsTrue(dt.WithinDowList("Monday", "Sat"));
    }

    [TestMethod]
    public void TestWithinDowList_StringParams_MatchByFullNameAfterNonMatch()
    {
        // "Mon" fails both sides of ||, then "Saturday" matches full-name (short-circuits ||)
        var dt = new DateTime(2026, 4, 18); // Saturday
        Assert.IsTrue(dt.WithinDowList("Mon", "Saturday"));
    }

    [TestMethod]
    public void TestWithinDowList_StringEnumerable()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        IEnumerable<string> dows = new List<string> { "Sat", "Sun" };
        Assert.IsTrue(dt.WithinDowList(dows));
    }

    [TestMethod]
    public void TestWithinDowList_StringEnumerable_NoMatch()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        IEnumerable<string> dows = new List<string> { "Mon", "Tue" };
        Assert.IsFalse(dt.WithinDowList(dows));
    }

    [TestMethod]
    public void TestWithinDowList_StringEnumerable_MatchByAbbreviatedAfterNonMatch()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        IEnumerable<string> dows = new List<string> { "Monday", "Sat" };
        Assert.IsTrue(dt.WithinDowList(dows));
    }

    [TestMethod]
    public void TestWithinDowList_StringEnumerable_MatchByFullNameAfterNonMatch()
    {
        var dt = new DateTime(2026, 4, 18); // Saturday
        IEnumerable<string> dows = new List<string> { "Mon", "Saturday" };
        Assert.IsTrue(dt.WithinDowList(dows));
    }
}
