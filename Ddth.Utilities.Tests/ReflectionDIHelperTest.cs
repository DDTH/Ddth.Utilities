using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ddth.Utilities.Tests;
[TestClass]
public class ReflectionDIHelperTest
{
    private IServiceProvider _serviceProvider = default!;

    [TestInitialize]
    public void Setup()
    {
        _serviceProvider = new ServiceCollection()
            .AddSingleton<IDummyName>(new DummyName())
            .AddSingleton("Dummy String")
            .BuildServiceProvider();
    }

    [TestMethod]
    public void TestCreateInstance()
    {
        var obj = ReflectionDIHelper.CreateInstance<DummyClass>(
            _serviceProvider,
            typeof(DummyClass)
        );
        Assert.IsNotNull(obj);
        Assert.AreEqual("Default", obj.Name);
    }

    [TestMethod]
    public void TestCreateInstanceWithConstructor()
    {
        var obj = ReflectionDIHelper.CreateInstance<DummyClassWithConstructor>(
            _serviceProvider,
            typeof(DummyClassWithConstructor)
        );
        Assert.IsNotNull(obj);
        Assert.AreEqual("Dummy", obj.Name);
    }

    [TestMethod]
    public void TestCreateInstanceWithSelectedConstructorTypeArr()
    {
        var obj = ReflectionDIHelper.CreateInstance<DummyClassWithMultipleConstructor>(
            _serviceProvider,
            typeof(DummyClassWithMultipleConstructor),
            new Type[] { typeof(string) }
        );
        Assert.IsNotNull(obj);
        Assert.AreEqual("constructor 2", obj.Name);
    }

    [TestMethod]
    public void TestCreateInstanceWithSelectedConstructorParameterInfoArr()
    {
        var pinfo = typeof(DummyClassWithMultipleConstructor).GetConstructors()[1].GetParameters()[0];
        var obj = ReflectionDIHelper.CreateInstance<DummyClassWithMultipleConstructor>(
            _serviceProvider,
            typeof(DummyClassWithMultipleConstructor),
            new ParameterInfo[] { pinfo }
        );
        Assert.IsNotNull(obj);
        Assert.AreEqual("constructor 2", obj.Name);
    }
}

public class DummyClass
{
    public string Name { get; set; } = "Default";
}

public interface IDummyName
{
    string Name { get; }
}

public class DummyName : IDummyName
{
    public string Name => "Dummy";
}

public class DummyClassWithConstructor
{
    public DummyClassWithConstructor(IDummyName name)
    {
        Name = name.Name;
    }

    public string Name { get; set; }
}

public class DummyClassWithMultipleConstructor
{
    public DummyClassWithMultipleConstructor(IDummyName _)
    {
        Name = "constructor 1";
    }

    public DummyClassWithMultipleConstructor(string _)
    {
        Name = "constructor 2";
    }

    public string Name { get; set; }
}
