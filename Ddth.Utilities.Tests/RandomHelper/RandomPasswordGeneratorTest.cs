using Microsoft.AspNetCore.Identity;

namespace Ddth.Utilities.Tests.RandomHelper;

public class RandomPasswordGeneratorTest
{
    const int NUM_ITERATIONS = 100000;

    [Fact]
    public void TestGenerateRandomPassword()
    {
        var options = RandomPasswordGenerator.DefaultPasswordOptions;
        for (var i = 0; i < NUM_ITERATIONS; i++)
        {
            var password = RandomPasswordGenerator.GenerateRandomPassword();
            Assert.NotNull(password);
            Assert.Equal(options.RequiredLength, password.Length);
            if (options.RequireDigit)
                Assert.Contains(password, ch => char.IsDigit(ch));
            if (options.RequireLowercase)
                Assert.Contains(password, ch => char.IsLower(ch));
            if (options.RequireUppercase)
                Assert.Contains(password, ch => char.IsUpper(ch));
            if (options.RequireNonAlphanumeric)
                Assert.Contains(password, ch => !char.IsLetterOrDigit(ch));
            Assert.True(password.Distinct().Count() >= options.RequiredUniqueChars);
        }
    }

    private readonly PasswordOptions options = new()
    {
        RequiredLength = 32,
        RequireDigit = true,
        RequireLowercase = true,
        RequireUppercase = true,
        RequireNonAlphanumeric = true,
        RequiredUniqueChars = 10,
    };

    [Fact]
    public void TestGenerateRandomPasswordWithCustomOptions()
    {
        for (var i = 0; i < NUM_ITERATIONS; i++)
        {
            var password = RandomPasswordGenerator.GenerateRandomPassword(options);
            Assert.NotNull(password);
            Assert.Equal(options.RequiredLength, password.Length);
            if (options.RequireDigit)
                Assert.Contains(password, ch => char.IsDigit(ch));
            if (options.RequireLowercase)
                Assert.Contains(password, ch => char.IsLower(ch));
            if (options.RequireUppercase)
                Assert.Contains(password, ch => char.IsUpper(ch));
            if (options.RequireNonAlphanumeric)
                Assert.Contains(password, ch => !char.IsLetterOrDigit(ch));
            Assert.True(password.Distinct().Count() >= options.RequiredUniqueChars);
        }
    }

    [Fact]
    public void TestGenerateRandomPasswordWithCustomLowercaseChars()
    {
        var lowercaseChars = "abcdef";
        var password = RandomPasswordGenerator.GenerateRandomPassword(options,
            lowercaseChars: lowercaseChars,
            uppercaseChars: string.Empty,
            digitChars: string.Empty,
            specialChars: string.Empty);
        Assert.NotNull(password);
        Assert.Equal(options.RequiredLength, password.Length);
        if (options.RequireDigit)
            Assert.Contains(password, ch => char.IsDigit(ch));
        if (options.RequireLowercase)
            Assert.Contains(password, ch => char.IsLower(ch));
        if (options.RequireUppercase)
            Assert.Contains(password, ch => char.IsUpper(ch));
        if (options.RequireNonAlphanumeric)
            Assert.Contains(password, ch => !char.IsLetterOrDigit(ch));
        Assert.True(password.Distinct().Count() >= options.RequiredUniqueChars);
        Assert.All(password, ch => Assert.True(!char.IsLower(ch) || lowercaseChars.Contains(ch, StringComparison.InvariantCulture)));
    }

    [Fact]
    public void TestGenerateRandomPasswordWithCustomUppercaseChars()
    {
        var uppercaseChars = "ABCDEF";
        var password = RandomPasswordGenerator.GenerateRandomPassword(options,
            lowercaseChars: string.Empty,
            uppercaseChars: uppercaseChars,
            digitChars: string.Empty,
            specialChars: string.Empty);
        Assert.NotNull(password);
        Assert.Equal(options.RequiredLength, password.Length);
        if (options.RequireDigit)
            Assert.Contains(password, ch => char.IsDigit(ch));
        if (options.RequireLowercase)
            Assert.Contains(password, ch => char.IsLower(ch));
        if (options.RequireUppercase)
            Assert.Contains(password, ch => char.IsUpper(ch));
        if (options.RequireNonAlphanumeric)
            Assert.Contains(password, ch => !char.IsLetterOrDigit(ch));
        Assert.True(password.Distinct().Count() >= options.RequiredUniqueChars);
        Assert.All(password, ch => Assert.True(!char.IsUpper(ch) || uppercaseChars.Contains(ch, StringComparison.InvariantCulture)));
    }

    [Fact]
    public void TestGenerateRandomPasswordWithCustomDigitChars()
    {
        var digitChars = "012345";
        var password = RandomPasswordGenerator.GenerateRandomPassword(options,
            lowercaseChars: string.Empty,
            uppercaseChars: string.Empty,
            digitChars: digitChars,
            specialChars: string.Empty);
        Assert.NotNull(password);
        Assert.Equal(options.RequiredLength, password.Length);
        if (options.RequireDigit)
            Assert.Contains(password, ch => char.IsDigit(ch));
        if (options.RequireLowercase)
            Assert.Contains(password, ch => char.IsLower(ch));
        if (options.RequireUppercase)
            Assert.Contains(password, ch => char.IsUpper(ch));
        if (options.RequireNonAlphanumeric)
            Assert.Contains(password, ch => !char.IsLetterOrDigit(ch));
        Assert.True(password.Distinct().Count() >= options.RequiredUniqueChars);
        Assert.All(password, ch => Assert.True(!char.IsDigit(ch) || digitChars.Contains(ch, StringComparison.InvariantCulture)));
    }

    [Fact]
    public void TestGenerateRandomPasswordWithCustomSpecialChars()
    {
        var specialChars = "!@#$%^";
        var password = RandomPasswordGenerator.GenerateRandomPassword(options,
            lowercaseChars: string.Empty,
            uppercaseChars: string.Empty,
            digitChars: string.Empty,
            specialChars: specialChars);
        Assert.NotNull(password);
        Assert.Equal(options.RequiredLength, password.Length);
        if (options.RequireDigit)
            Assert.Contains(password, ch => char.IsDigit(ch));
        if (options.RequireLowercase)
            Assert.Contains(password, ch => char.IsLower(ch));
        if (options.RequireUppercase)
            Assert.Contains(password, ch => char.IsUpper(ch));
        if (options.RequireNonAlphanumeric)
            Assert.Contains(password, ch => !char.IsLetterOrDigit(ch));
        Assert.True(password.Distinct().Count() >= options.RequiredUniqueChars);
        Assert.All(password, ch => Assert.True(char.IsLetterOrDigit(ch) || specialChars.Contains(ch, StringComparison.InvariantCulture)));
    }

    [Fact]
    public void TestGenerateRandomPasswordThrowException()
    {
        var options = new PasswordOptions
        {
            RequiredUniqueChars = 8,
        };
        Assert.Throws<ArgumentException>(() => RandomPasswordGenerator.GenerateRandomPassword(options, lowercaseChars: "a", uppercaseChars: "A", digitChars: "0", specialChars: "*"));
    }
}
