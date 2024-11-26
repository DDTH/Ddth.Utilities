using System.Security.Cryptography;

namespace Ddth.Utilities;

/// <summary>
/// Utility class to generate random values using cryptographically strong value generator <see cref="RandomNumberGenerator"/>.
/// </summary>
public static class RandomUtils
{
    private static readonly RandomNumberGenerator _random = RandomNumberGenerator.Create();

    /// <summary>
    /// Generates a random <c>char</c> from the given set of characters.
    /// </summary>
    /// <param name="chars">The set of allowed characters to generate from.</param>
    /// <returns></returns>
    /// <remarks>
    ///     If <c>chars</c> is null or empty, <c>'\0'</c> is returned.
    /// </remarks>
    public static char Next(string chars)
    {
        if (string.IsNullOrEmpty(chars))
        {
            return '\0';
        }
        var distinctChars = chars.Distinct().ToArray();
        return distinctChars[Next(0, distinctChars.Length)];
    }

    /// <summary>
    /// Generates a random <c>int</c>.
    /// </summary>
    /// <param name="minValue">minimum value to generate (inclusive).</param>
    /// <param name="maxValue">maximum value to generate (exclusive).</param>
    /// <returns>The random <c>int</c> in range <c>[minValue, maxValue)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <c>minValue</c> is greater than <c>maxValue</c>.</exception>
    /// <remarks>
    ///     <c>minValue</c> must be less than or equal to <c>maxValue</c>.
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
    /// Generates a random <c>short</c>.
    /// </summary>
    /// <param name="minValue">minimum value to generate (inclusive).</param>
    /// <param name="maxValue">maximum value to generate (exclusive).</param>
    /// <returns>The random <c>short</c> in range <c>[minValue, maxValue)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <c>minValue</c> is greater than <c>maxValue</c>.</exception>
    /// <remarks>
    ///     <c>minValue</c> must be less than or equal to <c>maxValue</c>.
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
    /// Generates a random <c>long</c>.
    /// </summary>
    /// <param name="minValue">minimum value to generate (inclusive).</param>
    /// <param name="maxValue">maximum value to generate (exclusive).</param>
    /// <returns>The random <c>long</c> in range <c>[minValue, maxValue)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <c>minValue</c> is greater than <c>maxValue</c>.</exception>
    /// <remarks>
    ///     <c>minValue</c> must be less than or equal to <c>maxValue</c>.
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
