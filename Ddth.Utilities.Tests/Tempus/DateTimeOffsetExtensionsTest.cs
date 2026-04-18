using Ddth.Utilities.Tempus;

namespace Ddth.Utilities.Tests.Tempus;

[TestClass]
public class DateTimeOffsetExtensionsTest
{
    /*----------------------------------------------------------------------*/
    /* StartOfDay                                                           */
    /*----------------------------------------------------------------------*/

    [TestMethod]
    public void TestStartOfDay_ZeroesTimeComponent()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 14, 30, 45, 123, TimeSpan.FromHours(10));
        var result = dt.StartOfDay();
        Assert.AreEqual(0, result.Hour);
        Assert.AreEqual(0, result.Minute);
        Assert.AreEqual(0, result.Second);
        Assert.AreEqual(0, result.Millisecond);
    }

    [TestMethod]
    public void TestStartOfDay_PreservesDateAndOffset()
    {
        var offset = TimeSpan.FromHours(5.5);
        var dt = new DateTimeOffset(2026, 12, 31, 23, 59, 59, offset);
        var result = dt.StartOfDay();
        Assert.AreEqual(2026, result.Year);
        Assert.AreEqual(12, result.Month);
        Assert.AreEqual(31, result.Day);
        Assert.AreEqual(offset, result.Offset);
    }

    [TestMethod]
    public void TestStartOfDay_AlreadyMidnight()
    {
        var dt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var result = dt.StartOfDay();
        Assert.AreEqual(dt, result);
    }

    /*----------------------------------------------------------------------*/
    /* PrevWeekDay                                                          */
    /*----------------------------------------------------------------------*/

    [TestMethod]
    public void TestPrevWeekDay_FromWeekday()
    {
        // Wednesday 2026-04-15
        var dt = new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero);
        var result = dt.PrevWeekDay();
        Assert.AreEqual(DayOfWeek.Tuesday, result.DayOfWeek);
    }

    [TestMethod]
    public void TestPrevWeekDay_FromMonday_SkipsWeekend()
    {
        // Monday 2026-04-13
        var dt = new DateTimeOffset(2026, 4, 13, 12, 0, 0, TimeSpan.Zero);
        Assert.AreEqual(DayOfWeek.Monday, dt.DayOfWeek);
        var result = dt.PrevWeekDay();
        Assert.AreEqual(DayOfWeek.Friday, result.DayOfWeek);
        Assert.AreEqual(10, result.Day);
    }

    [TestMethod]
    public void TestPrevWeekDay_FromSunday()
    {
        // Sunday 2026-04-12
        var dt = new DateTimeOffset(2026, 4, 12, 12, 0, 0, TimeSpan.Zero);
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
        var dt = new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero);
        var result = dt.NextWeekDay();
        Assert.AreEqual(DayOfWeek.Thursday, result.DayOfWeek);
    }

    [TestMethod]
    public void TestNextWeekDay_FromFriday_SkipsWeekend()
    {
        // Friday 2026-04-17
        var dt = new DateTimeOffset(2026, 4, 17, 12, 0, 0, TimeSpan.Zero);
        Assert.AreEqual(DayOfWeek.Friday, dt.DayOfWeek);
        var result = dt.NextWeekDay();
        Assert.AreEqual(DayOfWeek.Monday, result.DayOfWeek);
        Assert.AreEqual(20, result.Day);
    }

    [TestMethod]
    public void TestNextWeekDay_FromSaturday()
    {
        // Saturday 2026-04-18
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.AreEqual(DayOfWeek.Saturday, dt.DayOfWeek);
        var result = dt.NextWeekDay();
        Assert.AreEqual(DayOfWeek.Monday, result.DayOfWeek);
    }

    /*----------------------------------------------------------------------*/
    /* WithinTimeWindow                                                     */
    /*----------------------------------------------------------------------*/

    [TestMethod]
    public void TestWithinTimeWindow_InsideNormalWindow()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 10, 0, 0, TimeSpan.Zero);
        Assert.IsTrue(dt.WithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_OutsideNormalWindow()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 18, 0, 0, TimeSpan.Zero);
        Assert.IsFalse(dt.WithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_BeforeStart()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 7, 0, 0, TimeSpan.Zero);
        Assert.IsFalse(dt.WithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_AtStartBoundary_Inclusive()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 9, 0, 0, TimeSpan.Zero);
        Assert.IsTrue(dt.WithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_AtEndBoundary_Exclusive()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 17, 0, 0, TimeSpan.Zero);
        Assert.IsFalse(dt.WithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(17, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_AcrossMidnight_BeforeMidnight()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 23, 0, 0, TimeSpan.Zero);
        Assert.IsTrue(dt.WithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_AcrossMidnight_AfterMidnight()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 1, 0, 0, TimeSpan.Zero);
        Assert.IsTrue(dt.WithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_AcrossMidnight_Outside()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 15, 0, 0, TimeSpan.Zero);
        Assert.IsFalse(dt.WithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_StartEqualsEnd_AlwaysFalse()
    {
        var exact = new DateTimeOffset(2026, 4, 18, 9, 0, 0, TimeSpan.Zero);
        Assert.IsFalse(exact.WithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(9, 0)));

        var other = new DateTimeOffset(2026, 4, 18, 15, 0, 0, TimeSpan.Zero);
        Assert.IsFalse(other.WithinTimeWindow(new TimeOnly(9, 0), new TimeOnly(9, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_AcrossMidnight_AtStartBoundary_Inclusive()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 22, 0, 0, TimeSpan.Zero);
        Assert.IsTrue(dt.WithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    [TestMethod]
    public void TestWithinTimeWindow_AcrossMidnight_AtEndBoundary_Exclusive()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 2, 0, 0, TimeSpan.Zero);
        Assert.IsFalse(dt.WithinTimeWindow(new TimeOnly(22, 0), new TimeOnly(2, 0)));
    }

    /*----------------------------------------------------------------------*/
    /* WithinDowList                                                        */
    /*----------------------------------------------------------------------*/

    [TestMethod]
    public void TestWithinDowList_DayOfWeekParams_Match()
    {
        // Saturday
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.IsTrue(dt.WithinDowList(DayOfWeek.Saturday, DayOfWeek.Sunday));
    }

    [TestMethod]
    public void TestWithinDowList_DayOfWeekParams_NoMatch()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.IsFalse(dt.WithinDowList(DayOfWeek.Monday, DayOfWeek.Tuesday));
    }

    [TestMethod]
    public void TestWithinDowList_DayOfWeekEnumerable()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        IEnumerable<DayOfWeek> dows = new List<DayOfWeek> { DayOfWeek.Saturday };
        Assert.IsTrue(dt.WithinDowList(dows));
    }

    [TestMethod]
    public void TestWithinDowList_StringParams_FullName()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.IsTrue(dt.WithinDowList("Saturday"));
    }

    [TestMethod]
    public void TestWithinDowList_StringParams_Abbreviated()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.IsTrue(dt.WithinDowList("Sat"));
    }

    [TestMethod]
    public void TestWithinDowList_StringParams_CaseInsensitive()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.IsTrue(dt.WithinDowList("saturday"));
        Assert.IsTrue(dt.WithinDowList("SATURDAY"));
        Assert.IsTrue(dt.WithinDowList("sat"));
    }

    [TestMethod]
    public void TestWithinDowList_StringParams_NoMatch()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.IsFalse(dt.WithinDowList("Monday", "Tuesday"));
    }

    [TestMethod]
    public void TestWithinDowList_StringParams_MatchByAbbreviatedAfterNonMatch()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.IsTrue(dt.WithinDowList("Monday", "Sat"));
    }

    [TestMethod]
    public void TestWithinDowList_StringParams_MatchByFullNameAfterNonMatch()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        Assert.IsTrue(dt.WithinDowList("Mon", "Saturday"));
    }

    [TestMethod]
    public void TestWithinDowList_StringEnumerable()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        IEnumerable<string> dows = new List<string> { "Sat", "Sun" };
        Assert.IsTrue(dt.WithinDowList(dows));
    }

    [TestMethod]
    public void TestWithinDowList_StringEnumerable_NoMatch()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        IEnumerable<string> dows = new List<string> { "Mon", "Tue" };
        Assert.IsFalse(dt.WithinDowList(dows));
    }

    [TestMethod]
    public void TestWithinDowList_StringEnumerable_MatchByAbbreviatedAfterNonMatch()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        IEnumerable<string> dows = new List<string> { "Monday", "Sat" };
        Assert.IsTrue(dt.WithinDowList(dows));
    }

    [TestMethod]
    public void TestWithinDowList_StringEnumerable_MatchByFullNameAfterNonMatch()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        IEnumerable<string> dows = new List<string> { "Mon", "Saturday" };
        Assert.IsTrue(dt.WithinDowList(dows));
    }

    /*----------------------------------------------------------------------*/
    /* ToTimeZoneSilently                                                   */
    /*----------------------------------------------------------------------*/

    [TestMethod]
    public void TestToTimeZoneSilently_ValidTimeZone()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero); // UTC noon
        var result = dt.ToTimeZoneSilently("Australia/Sydney");
        Assert.IsNotNull(result);
        Assert.AreNotEqual(TimeSpan.Zero, result.Value.Offset);
    }

    [TestMethod]
    public void TestToTimeZoneSilently_PreservesInstant()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        var result = dt.ToTimeZoneSilently("Australia/Sydney");
        Assert.IsNotNull(result);
        Assert.AreEqual(dt.UtcDateTime, result.Value.UtcDateTime);
    }

    [TestMethod]
    public void TestToTimeZoneSilently_InvalidTimeZone_ReturnsNull()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        var result = dt.ToTimeZoneSilently("Invalid/TimeZone");
        Assert.IsNull(result);
    }

    /*----------------------------------------------------------------------*/
    /* AsTimeZoneSilently                                                   */
    /*----------------------------------------------------------------------*/

    [TestMethod]
    public void TestAsTimeZoneSilently_ValidTimeZone_KeepsDateTime()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        var result = dt.AsTimeZoneSilently("Australia/Sydney");
        Assert.IsNotNull(result);
        // The date and time components should remain the same
        Assert.AreEqual(dt.Year, result.Value.Year);
        Assert.AreEqual(dt.Month, result.Value.Month);
        Assert.AreEqual(dt.Day, result.Value.Day);
        Assert.AreEqual(dt.Hour, result.Value.Hour);
        Assert.AreEqual(dt.Minute, result.Value.Minute);
    }

    [TestMethod]
    public void TestAsTimeZoneSilently_ValidTimeZone_ChangesOffset()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        var result = dt.AsTimeZoneSilently("Australia/Sydney");
        Assert.IsNotNull(result);
        Assert.AreNotEqual(TimeSpan.Zero, result.Value.Offset);
    }

    [TestMethod]
    public void TestAsTimeZoneSilently_InvalidTimeZone_ReturnsNull()
    {
        var dt = new DateTimeOffset(2026, 4, 18, 12, 0, 0, TimeSpan.Zero);
        var result = dt.AsTimeZoneSilently("Invalid/TimeZone");
        Assert.IsNull(result);
    }
}
