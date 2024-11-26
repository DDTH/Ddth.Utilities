using System.Security.Cryptography;

namespace Ddth.Utilities;

/// <summary>
/// Utility class to generate random values using cryptographically strong value generator <see cref="RandomNumberGenerator"/>.
/// </summary>
public static class RandomUtils
{
    private static readonly RandomNumberGenerator _random = RandomNumberGenerator.Create();

    /// <summary>
    /// Generates a random char from the given set of characters.
    /// </summary>
    /// <param name="chars"></param>
    /// <returns></returns>
    /// <remarks>
    ///     if chars is null or empty, '\0' is returned
    /// </remarks>
    public static char Next(string chars)
    {
        if (string.IsNullOrEmpty(chars))
        {
            return '\0';
        }
        return chars[Next(0, chars.Length)];
    }

    /// <summary>
    /// Generates a random int.
    /// </summary>
    /// <param name="minValue">minimum value to generate (inclusive)</param>
    /// <param name="maxValue">maximum value to generate (exclusive)</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <remarks>
    ///     minValue must be less than or equal to maxValue<br/>
    ///     if minValue == maxValue, minValue is returned
    /// </remarks>
    public static int Next(int minValue, int maxValue)
    {
        if (minValue > maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(minValue), $"{nameof(minValue)} must be less than or equal to {nameof(maxValue)}");
        }
        if (minValue == maxValue)
        {
            return minValue;
        }
        var data = new byte[sizeof(int)];
        _random.GetBytes(data);
        var value = BitConverter.ToInt32(data, 0);
        return Math.Abs(value % (maxValue - minValue)) + minValue;
    }

    /// <summary>
    /// Generates a random short.
    /// </summary>
    /// <param name="minValue">minimum value to generate (inclusive)</param>
    /// <param name="maxValue">maximum value to generate (exclusive)</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <remarks>
    ///     minValue must be less than or equal to maxValue<br/>
    ///     if minValue == maxValue, minValue is returned
    /// </remarks>
    public static short Next(short minValue, short maxValue)
    {
        if (minValue > maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(minValue), $"{nameof(minValue)} must be less than or equal to {nameof(maxValue)}");
        }
        if (minValue == maxValue)
        {
            return minValue;
        }
        var data = new byte[sizeof(short)];
        _random.GetBytes(data);
        var value = BitConverter.ToInt16(data, 0);
        return (short)(Math.Abs(value % (maxValue - minValue)) + minValue);
    }

    /// <summary>
    /// Generates a random long.
    /// </summary>
    /// <param name="minValue">minimum value to generate (inclusive)</param>
    /// <param name="maxValue">maximum value to generate (exclusive)</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <remarks>
    ///     minValue must be less than or equal to maxValue<br/>
    ///     if minValue == maxValue, minValue is returned
    /// </remarks>
    public static long Next(long minValue, long maxValue)
    {
        if (minValue > maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(minValue), $"{nameof(minValue)} must be less than or equal to {nameof(maxValue)}");
        }
        if (minValue == maxValue)
        {
            return minValue;
        }
        var data = new byte[sizeof(long)];
        _random.GetBytes(data);
        var value = BitConverter.ToInt64(data, 0);
        return Math.Abs(value % (maxValue - minValue)) + minValue;
    }
}
