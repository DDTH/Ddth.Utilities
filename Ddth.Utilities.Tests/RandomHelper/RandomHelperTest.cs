namespace Ddth.Utilities.Tests.RandomHelper;

[TestClass]
public class RandomHelperTest
{
    const int NUM_ITERATIONS = 1000000;
    const int RANGE = 1000;

    [TestMethod]
    public void TestRandomCharEmptyChars()
    {
        Assert.AreEqual('\0', Utilities.RandomUtils.Next(string.Empty));
    }

    [TestMethod]
    public void TestRandomChar()
    {
        var inChars = "abcdef";
        var outChars = "xyzt";
        for (var i = 0; i < NUM_ITERATIONS; i++)
        {
            var randomChar = Utilities.RandomUtils.Next(inChars);
            Assert.IsTrue(inChars.Contains(randomChar, StringComparison.Ordinal));
            Assert.IsFalse(outChars.Contains(randomChar, StringComparison.Ordinal));
        }
    }

    [TestMethod]
    public void TestRandomIntInvalidInputs()
    {
        var minValue = 100;
        var maxValue = minValue - 1;
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Utilities.RandomUtils.Next(minValue, maxValue));
    }

    [TestMethod]
    public void TestRandomInt()
    {
        var minValue = -10;
        for (var i = 0; i < NUM_ITERATIONS; i++)
        {
            var maxValue = minValue + Math.Min(i, RANGE);
            var randomInt = Utilities.RandomUtils.Next(minValue, maxValue);
            Assert.IsTrue(minValue == maxValue ? randomInt == minValue : randomInt >= minValue && randomInt < maxValue);
        }
    }

    [TestMethod]
    public void TestRandomShortInvalidInputs()
    {
        var minValue = (short)100;
        var maxValue = (short)(minValue - 1);
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Utilities.RandomUtils.Next(minValue, maxValue));
    }

    [TestMethod]
    public void TestRandomShort()
    {
        short minValue = -10;
        for (var i = 0; i < NUM_ITERATIONS; i++)
        {
            var maxValue = (short)(minValue + Math.Min(i, RANGE));
            var randomShort = Utilities.RandomUtils.Next(minValue, maxValue);
            Assert.IsTrue(minValue == maxValue ? randomShort == minValue : randomShort >= minValue && randomShort < maxValue);
        }
    }

    [TestMethod]
    public void TestRandomLongInvalidInputs()
    {
        var minValue = 100L;
        var maxValue = minValue - 1;
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Utilities.RandomUtils.Next(minValue, maxValue));
    }

    [TestMethod]
    public void TestRandomLong()
    {
        long minValue = -10;
        for (var i = 0; i < NUM_ITERATIONS; i++)
        {
            var maxValue = minValue + Math.Min(i, RANGE);
            var randomLong = Utilities.RandomUtils.Next(minValue, maxValue);
            Assert.IsTrue(minValue == maxValue ? randomLong == minValue : randomLong >= minValue && randomLong < maxValue);
        }
    }
}
