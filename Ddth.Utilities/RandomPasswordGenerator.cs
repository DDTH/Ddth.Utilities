using Microsoft.AspNetCore.Identity;

namespace Ddth.Utilities;

/// <summary>
/// Helper class to generate random passwords, respecting the given strength requirements.
/// </summary>
public static class RandomPasswordGenerator
{
    /// <summary>
    /// Default password strength requirements.
    /// </summary>
    public static readonly PasswordOptions DefaultPasswordOptions = new()
    {
        RequiredLength = 12,
        RequiredUniqueChars = 5,
        RequireDigit = true,
        RequireLowercase = true,
        RequireNonAlphanumeric = false,
        RequireUppercase = true
    };

    public const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    public const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string DigitChars = "0123456789";
    public const string SpecialChars = "!@#$%^&*()_-+=[{]};:>|./?";

    /// <summary>
    /// Generates a random password with default strength requirements <see cref="DefaultPasswordOptions"/>.
    /// </summary>
    /// <returns></returns>
    public static string GenerateRandomPassword() => GenerateRandomPassword(null);

    /// <summary>
    /// Generates a random password, respecting the given strength requirements.
    /// </summary>
    /// <param name="passwordOptions">A valid <see cref="PasswordOptions"/> object containing the password strength requirements.</param>
    /// <param name="lowercaseChars"></param>
    /// <param name="uppercaseChars"></param>
    /// <param name="digitChars"></param>
    /// <param name="specialChars"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <remarks>Credits: https://www.ryadel.com/en/c-sharp-random-password-generator-asp-net-core-mvc/</remarks>
    public static string GenerateRandomPassword(
        PasswordOptions? passwordOptions,
        string lowercaseChars = LowercaseChars,
        string uppercaseChars = UppercaseChars,
        string digitChars = DigitChars,
        string specialChars = SpecialChars)
    {
        passwordOptions ??= DefaultPasswordOptions;
        var charSet = new[]
        {
            string.IsNullOrEmpty(lowercaseChars) ? LowercaseChars : lowercaseChars,
            string.IsNullOrEmpty(uppercaseChars) ? UppercaseChars : uppercaseChars,
            string.IsNullOrEmpty(digitChars) ? DigitChars : digitChars,
            string.IsNullOrEmpty(specialChars) ? SpecialChars : specialChars
        };

        if ($"{charSet[0]}{charSet[1]}{charSet[2]}{charSet[3]}".Distinct().Count() < passwordOptions.RequiredUniqueChars)
        {
            throw new ArgumentException("Not enough unique characters in the character set");
        }

        var chars = new List<char>();
        if (passwordOptions.RequireLowercase)
        {
            chars.Insert(RandomUtils.Next(0, chars.Count), RandomUtils.Next(charSet[0]));
        }
        if (passwordOptions.RequireUppercase)
        {
            chars.Insert(RandomUtils.Next(0, chars.Count), RandomUtils.Next(charSet[1]));
        }
        if (passwordOptions.RequireDigit)
        {
            chars.Insert(RandomUtils.Next(0, chars.Count), RandomUtils.Next(charSet[2]));
        }
        if (passwordOptions.RequireNonAlphanumeric)
        {
            chars.Insert(RandomUtils.Next(0, chars.Count), RandomUtils.Next(charSet[3]));
        }

        var numSet = passwordOptions.RequireNonAlphanumeric ? charSet.Length : charSet.Length - 1;
        for (var i = chars.Count; i < passwordOptions.RequiredLength || chars.Distinct().Count() < passwordOptions.RequiredUniqueChars; i++)
        {
            var randomCharSet = charSet[RandomUtils.Next(0, numSet)];
            chars.Insert(RandomUtils.Next(0, chars.Count), RandomUtils.Next(randomCharSet));
        }

        return new string(chars.ToArray());
    }
}
