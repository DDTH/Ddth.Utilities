using Microsoft.AspNetCore.Identity;

namespace Ddth.Utilities.Tests.RandomHelper;

[TestClass]
public class RandomPasswordGeneratorTest
{
    const int NUM_ITERATIONS = 100000;

    [TestMethod]
    public void TestGenerateRandomPassword()
    {
        var options = RandomPasswordGenerator.DefaultPasswordOptions;
        for (var i = 0; i < NUM_ITERATIONS; i++)
        {
            var password = RandomPasswordGenerator.GenerateRandomPassword();
            Assert.IsNotNull(password);
            Assert.AreEqual(password.Length, options.RequiredLength);
            if (options.RequireDigit)
                Assert.IsTrue(password.Any(char.IsDigit));
            if (options.RequireLowercase)
                Assert.IsTrue(password.Any(char.IsLower));
            if (options.RequireUppercase)
                Assert.IsTrue(password.Any(char.IsUpper));
            if (options.RequireNonAlphanumeric)
                Assert.IsTrue(password.Any(ch => !char.IsLetterOrDigit(ch)));
            Assert.IsTrue(password.Distinct().Count() >= options.RequiredUniqueChars);
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

    [TestMethod]
    public void TestGenerateRandomPasswordWithCustomOptions()
    {
        for (var i = 0; i < NUM_ITERATIONS; i++)
        {
            var password = RandomPasswordGenerator.GenerateRandomPassword(options);
            Assert.IsNotNull(password);
            Assert.AreEqual(password.Length, options.RequiredLength);
            if (options.RequireDigit)
                Assert.IsTrue(password.Any(char.IsDigit));
            if (options.RequireLowercase)
                Assert.IsTrue(password.Any(char.IsLower));
            if (options.RequireUppercase)
                Assert.IsTrue(password.Any(char.IsUpper));
            if (options.RequireNonAlphanumeric)
                Assert.IsTrue(password.Any(ch => !char.IsLetterOrDigit(ch)));
            Assert.IsTrue(password.Distinct().Count() >= options.RequiredUniqueChars);
        }
    }

    [TestMethod]
    public void TestGenerateRandomPasswordWithCustomLowercaseChars()
    {
        var lowercaseChars = "abcdef";
        var password = RandomPasswordGenerator.GenerateRandomPassword(options,
            lowercaseChars: lowercaseChars,
            uppercaseChars: string.Empty,
            digitChars: string.Empty,
            specialChars: string.Empty);
        Assert.IsNotNull(password);
        Assert.AreEqual(password.Length, options.RequiredLength);
        if (options.RequireDigit)
            Assert.IsTrue(password.Any(char.IsDigit));
        if (options.RequireLowercase)
            Assert.IsTrue(password.Any(char.IsLower));
        if (options.RequireUppercase)
            Assert.IsTrue(password.Any(char.IsUpper));
        if (options.RequireNonAlphanumeric)
            Assert.IsTrue(password.Any(ch => !char.IsLetterOrDigit(ch)));
        Assert.IsTrue(password.Distinct().Count() >= options.RequiredUniqueChars);
        Assert.IsTrue(password.All(ch => !char.IsLower(ch) || lowercaseChars.Contains(ch, StringComparison.InvariantCulture)));
    }

    [TestMethod]
    public void TestGenerateRandomPasswordWithCustomUppercaseChars()
    {
        var uppercaseChars = "ABCDEF";
        var password = RandomPasswordGenerator.GenerateRandomPassword(options,
            lowercaseChars: string.Empty,
            uppercaseChars: uppercaseChars,
            digitChars: string.Empty,
            specialChars: string.Empty);
        Assert.IsNotNull(password);
        Assert.AreEqual(password.Length, options.RequiredLength);
        if (options.RequireDigit)
            Assert.IsTrue(password.Any(char.IsDigit));
        if (options.RequireLowercase)
            Assert.IsTrue(password.Any(char.IsLower));
        if (options.RequireUppercase)
            Assert.IsTrue(password.Any(char.IsUpper));
        if (options.RequireNonAlphanumeric)
            Assert.IsTrue(password.Any(ch => !char.IsLetterOrDigit(ch)));
        Assert.IsTrue(password.Distinct().Count() >= options.RequiredUniqueChars);
        Assert.IsTrue(password.All(ch => !char.IsUpper(ch) || uppercaseChars.Contains(ch, StringComparison.InvariantCulture)));
    }

    [TestMethod]
    public void TestGenerateRandomPasswordWithCustomDigitChars()
    {
        var digitChars = "012345";
        var password = RandomPasswordGenerator.GenerateRandomPassword(options,
            lowercaseChars: string.Empty,
            uppercaseChars: string.Empty,
            digitChars: digitChars,
            specialChars: string.Empty);
        Assert.IsNotNull(password);
        Assert.AreEqual(password.Length, options.RequiredLength);
        if (options.RequireDigit)
            Assert.IsTrue(password.Any(char.IsDigit));
        if (options.RequireLowercase)
            Assert.IsTrue(password.Any(char.IsLower));
        if (options.RequireUppercase)
            Assert.IsTrue(password.Any(char.IsUpper));
        if (options.RequireNonAlphanumeric)
            Assert.IsTrue(password.Any(ch => !char.IsLetterOrDigit(ch)));
        Assert.IsTrue(password.Distinct().Count() >= options.RequiredUniqueChars);
        Assert.IsTrue(password.All(ch => !char.IsDigit(ch) || digitChars.Contains(ch, StringComparison.InvariantCulture)));
    }

    [TestMethod]
    public void TestGenerateRandomPasswordWithCustomSpecialChars()
    {
        var specialChars = "!@#$%^";
        var password = RandomPasswordGenerator.GenerateRandomPassword(options,
            lowercaseChars: string.Empty,
            uppercaseChars: string.Empty,
            digitChars: string.Empty,
            specialChars: specialChars);
        Assert.IsNotNull(password);
        Assert.AreEqual(password.Length, options.RequiredLength);
        if (options.RequireDigit)
            Assert.IsTrue(password.Any(char.IsDigit));
        if (options.RequireLowercase)
            Assert.IsTrue(password.Any(char.IsLower));
        if (options.RequireUppercase)
            Assert.IsTrue(password.Any(char.IsUpper));
        if (options.RequireNonAlphanumeric)
            Assert.IsTrue(password.Any(ch => !char.IsLetterOrDigit(ch)));
        Assert.IsTrue(password.Distinct().Count() >= options.RequiredUniqueChars);
        Assert.IsTrue(password.All(ch => char.IsLetterOrDigit(ch) || specialChars.Contains(ch, StringComparison.InvariantCulture)));
    }

    [TestMethod]
    public void TestGenerateRandomPasswordThrowException()
    {
        var options = new PasswordOptions
        {
            RequiredUniqueChars = 8,
        };
        Assert.ThrowsException<ArgumentException>(() => RandomPasswordGenerator.GenerateRandomPassword(options, lowercaseChars: "a", uppercaseChars: "A", digitChars: "0", specialChars: "*"));
    }
}
