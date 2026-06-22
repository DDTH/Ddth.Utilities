namespace Ddth.Utilities.Tests.RandomHelper;

public class RandomHelperTest
{
    const int NUM_ITERATIONS = 1000000;
    const int RANGE = 1000;

    [Fact]
    public void TestRandomCharEmptyChars()
    {
        Assert.Equal('\0', RandomUtils.Next(string.Empty));
    }

    [Fact]
    public void TestRandomChar()
    {
        var inChars = "abcdef";
        var outChars = "xyzt";
        for (var i = 0; i < NUM_ITERATIONS; i++)
        {
            var randomChar = RandomUtils.Next(inChars);
            Assert.True(inChars.Contains(randomChar, StringComparison.Ordinal));
            Assert.False(outChars.Contains(randomChar, StringComparison.Ordinal));
        }
    }

    [Fact]
    public void TestRandomIntInvalidInputs()
    {
        var minValue = 100;
        var maxValue = minValue - 1;
        Assert.Throws<ArgumentOutOfRangeException>(() => RandomUtils.Next(minValue, maxValue));
    }

    [Fact]
    public void TestRandomInt()
    {
        var minValue = -10;
        for (var i = 0; i < NUM_ITERATIONS; i++)
        {
            var maxValue = minValue + Math.Min(i, RANGE);
            var randomInt = RandomUtils.Next(minValue, maxValue);
            Assert.True(minValue == maxValue ? randomInt == minValue : randomInt >= minValue && randomInt < maxValue);
        }
    }

    [Fact]
    public void TestRandomShortInvalidInputs()
    {
        var minValue = (short)100;
        var maxValue = (short)(minValue - 1);
        Assert.Throws<ArgumentOutOfRangeException>(() => RandomUtils.Next(minValue, maxValue));
    }

    [Fact]
    public void TestRandomShort()
    {
        short minValue = -10;
        for (var i = 0; i < NUM_ITERATIONS; i++)
        {
            var maxValue = (short)(minValue + Math.Min(i, RANGE));
            var randomShort = RandomUtils.Next(minValue, maxValue);
            Assert.True(minValue == maxValue ? randomShort == minValue : randomShort >= minValue && randomShort < maxValue);
        }
    }

    [Fact]
    public void TestRandomLongInvalidInputs()
    {
        var minValue = 100L;
        var maxValue = minValue - 1;
        Assert.Throws<ArgumentOutOfRangeException>(() => RandomUtils.Next(minValue, maxValue));
    }

    [Fact]
    public void TestRandomLong()
    {
        long minValue = -10;
        for (var i = 0; i < NUM_ITERATIONS; i++)
        {
            var maxValue = minValue + Math.Min(i, RANGE);
            var randomLong = RandomUtils.Next(minValue, maxValue);
            Assert.True(minValue == maxValue ? randomLong == minValue : randomLong >= minValue && randomLong < maxValue);
        }
    }
}
