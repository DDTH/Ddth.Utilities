namespace Ddth.Utilities.Tests;
using Microsoft.Extensions.DependencyInjection;
using System;

[TestClass]
public class ReflectionDIHelperTest
{
    private IServiceProvider _serviceProvider = default!;

    [TestInitialize]
    public void Setup()
    {
        _serviceProvider = new ServiceCollection().BuildServiceProvider();
    }

    [TestMethod]
    public void TestCreateInstance()
    {
        var obj = ReflectionDIHelper.CreateInstance<DummyClass>(_serviceProvider, typeof(DummyClass));
        Assert.IsNotNull(obj);
        Assert.AreEqual("Default", obj.Name);
    }
}

public class DummyClass
{
    public string Name { get; set; } = "Default";
}
