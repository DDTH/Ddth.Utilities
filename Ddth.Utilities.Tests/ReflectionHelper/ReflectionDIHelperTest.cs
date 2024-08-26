using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ddth.Utilities.Tests.ReflectionHelper;

[TestClass]
public class ReflectionDIHelperTest
{
    private IServiceProvider _serviceProvider = default!;
    private IEnumerable<object?> _services = default!;

    [TestInitialize]
    public void Setup()
    {
        _serviceProvider = new ServiceCollection()
            .AddSingleton<IDummyService1>(new DummyServiceMultiInterfaces("1"))
            .AddSingleton(typeof(IDummyService2), new DummyServiceMultiInterfaces("2"))
            .AddSingleton(new DummyServiceMultiInterfaces("3"))
            .AddSingleton<IDummyName, DummyName>()
            .AddSingleton("Dummy String")
            .BuildServiceProvider();
        _services = new object?[] { new DummyServiceMultiInterfaces("0") };
    }

    /*----------------------------------------------------------------------*/

    [TestMethod]
    public void TestBuildDIParamsWithServiceProvider()
    {
        var parameters = new Type[] { typeof(IDummyService1), typeof(IDummyService2) };
        var result = ReflectionDIHelper.BuildDIParams(_serviceProvider, parameters);
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Length);
        Assert.IsInstanceOfType(result[0], typeof(IDummyService1));
        Assert.IsInstanceOfType(result[1], typeof(IDummyService2));
        Assert.AreEqual("1", ((IDummyService1)result[0]!).Name);
        Assert.AreEqual("2", ((IDummyService2)result[1]!).Name);
    }

    [TestMethod]
    public void TestBuildDIParamsWithNullServiceProvider()
    {
        var parameters = new Type[] { typeof(IDummyService1), typeof(IDummyService2) };
        var result = ReflectionDIHelper.BuildDIParams(null, _services, parameters);
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Length);
        Assert.IsInstanceOfType(result[0], typeof(IDummyService1));
        Assert.IsInstanceOfType(result[1], typeof(IDummyService2));
        Assert.AreEqual("0", ((IDummyService1)result[0]!).Name);
        Assert.AreEqual("0", ((IDummyService2)result[1]!).Name);
    }

    [TestMethod]
    public void TestBuildDIParamsFallbackToServiceProvider()
    {
        var parameters = new Type[] { typeof(string) };
        var result = ReflectionDIHelper.BuildDIParams(_serviceProvider, _services, parameters);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Length);
        Assert.IsInstanceOfType(result[0], typeof(string));
        Assert.AreEqual("Dummy String", ((string)result[0]!));
    }

    [TestMethod]
    public void TestBuildDIParamsNullResult()
    {
        var parameters = new Type[] { typeof(IUnusedInterface) };
        var result = ReflectionDIHelper.BuildDIParams(null, _services, parameters);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Length);
        Assert.IsNull(result[0]);
    }

    /*----------------------------------------------------------------------*/

    sealed class TestClass
    {
        public static void TestMethodNeedsDummyService1and2(IDummyService1 _1, IDummyService2 _2)
        {
        }

        public static void TestMethodNeedsString(string _)
        {
        }

        public static void TestMethodNeedsUnusedInterface(IUnusedInterface _)
        {
        }
    }

    [TestMethod]
    public void TestBuildDIParamsWithServiceProviderUsingParameterInfo()
    {
        var parameters = typeof(TestClass).GetMethod("TestMethodNeedsDummyService1and2")!.GetParameters();
        var result = ReflectionDIHelper.BuildDIParams(_serviceProvider, parameters);
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Length);
        Assert.IsInstanceOfType(result[0], typeof(IDummyService1));
        Assert.IsInstanceOfType(result[1], typeof(IDummyService2));
        Assert.AreEqual("1", ((IDummyService1)result[0]!).Name);
        Assert.AreEqual("2", ((IDummyService2)result[1]!).Name);
    }

    [TestMethod]
    public void TestBuildDIParamsWithNullServiceProviderUsingParameterInfo()
    {
        var parameters = typeof(TestClass).GetMethod("TestMethodNeedsDummyService1and2")!.GetParameters();
        var result = ReflectionDIHelper.BuildDIParams(null, _services, parameters);
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Length);
        Assert.IsInstanceOfType(result[0], typeof(IDummyService1));
        Assert.IsInstanceOfType(result[1], typeof(IDummyService2));
        Assert.AreEqual("0", ((IDummyService1)result[0]!).Name);
        Assert.AreEqual("0", ((IDummyService2)result[1]!).Name);
    }

    [TestMethod]
    public void TestBuildDIParamsFallbackToServiceProviderUsingParameterInfo()
    {
        var parameters = typeof(TestClass).GetMethod("TestMethodNeedsString")!.GetParameters();
        var result = ReflectionDIHelper.BuildDIParams(_serviceProvider, _services, parameters);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Length);
        Assert.IsInstanceOfType(result[0], typeof(string));
        Assert.AreEqual("Dummy String", ((string)result[0]!));
    }

    [TestMethod]
    public void TestBuildDIParamsNullResultUsingParameterInfo()
    {
        var parameters = typeof(TestClass).GetMethod("TestMethodNeedsUnusedInterface")!.GetParameters();
        var result = ReflectionDIHelper.BuildDIParams(null, _services, parameters);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Length);
        Assert.IsNull(result[0]);
    }

    /*----------------------------------------------------------------------*/

    [TestMethod]
    public void TestCreateInstanceNotAssignableShouldBeNull()
    {
        var obj = ReflectionDIHelper.CreateInstance<IUnusedInterface>(
            _serviceProvider,
            typeof(DummyClass)
        );
        Assert.IsNull(obj);
    }

    [TestMethod]
    public void TestCreateInstanceTypeClass()
    {
        var obj = ReflectionDIHelper.CreateInstance<DummyClass>(
            _serviceProvider,
            typeof(DummyClass)
        );
        Assert.IsNotNull(obj);
        Assert.AreEqual("Default", obj.Name);
    }

    [TestMethod]
    public void TestCreateInstanceTypeInterface()
    {
        var obj = ReflectionDIHelper.CreateInstance<IDummyInterface>(
            _serviceProvider,
            typeof(DummyClass)
        );
        Assert.IsNotNull(obj);
        Assert.AreEqual("Default", obj.Name);
    }

    [TestMethod]
    public void TestCreateInstanceTypeInterfaceFallback()
    {
        var obj = ReflectionDIHelper.CreateInstance<IDummyInterface>(
            _serviceProvider,
            _services,
            typeof(DummyClass)
        );
        Assert.IsNotNull(obj);
        Assert.AreEqual("Default", obj.Name);
    }

    /*----------------------------------------------------------------------*/

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
        var pinfo = typeof(DummyClassWithMultipleConstructor).GetConstructors()[0].GetParameters()[0];
        var obj = ReflectionDIHelper.CreateInstance<DummyClassWithMultipleConstructor>(
            _serviceProvider,
            typeof(DummyClassWithMultipleConstructor),
            new ParameterInfo[] { pinfo }
        );
        Assert.IsNotNull(obj);
        Assert.AreEqual("constructor 1", obj.Name);
    }

    [TestMethod]
    public void TestCreateInstanceWithSelectedConstructorTypeArrNotAssignableShouldBeNull()
    {
        var obj = ReflectionDIHelper.CreateInstance<IUnusedInterface>(
            _serviceProvider,
            typeof(DummyClassWithMultipleConstructor),
            new Type[] { typeof(string) }
        );
        Assert.IsNull(obj);
    }

    [TestMethod]
    public void TestCreateInstanceWithSelectedConstructorParameterInfoArrNotAssignableShouldBeNull()
    {
        var pinfo = typeof(DummyClassWithMultipleConstructor).GetConstructors()[0].GetParameters()[0];
        var obj = ReflectionDIHelper.CreateInstance<IUnusedInterface>(
            _serviceProvider,
            typeof(DummyClassWithMultipleConstructor),
            new ParameterInfo[] { pinfo }
        );
        Assert.IsNull(obj);
    }

    [TestMethod]
    public void TestCreateInstanceNeedIDummyService1()
    {
        var obj = ReflectionDIHelper.CreateInstance<IDummyService1>(
            _serviceProvider,
            typeof(DummyClassNeedIDummyService1)
        );
        Assert.IsNotNull(obj);
        Assert.AreEqual("1", obj.Name);
    }

    [TestMethod]
    public void TestCreateInstanceNeedIDummyService2()
    {
        var obj = ReflectionDIHelper.CreateInstance<IDummyService2>(
            _serviceProvider,
            typeof(DummyClassNeedIDummyService2)
        );
        Assert.IsNotNull(obj);
        Assert.AreEqual("2", obj.Name);
    }

    [TestMethod]
    public void TestCreateInstanceOfDummyService1HasIDummyService0()
    {
        var obj = ReflectionDIHelper.CreateInstance<IDummyService>(
            _serviceProvider,
            _services,
            typeof(DummyClassNeedIDummyService1)
        );
        Assert.IsNotNull(obj);
        Assert.AreEqual("0", obj.Name);
    }

    [TestMethod]
    public void TestCreateInstanceOfDummyService2HasIDummyService0()
    {
        var obj = ReflectionDIHelper.CreateInstance<IDummyService>(
            _serviceProvider,
            _services,
            typeof(DummyClassNeedIDummyService2)
        );
        Assert.IsNotNull(obj);
        Assert.AreEqual("0", obj.Name);
    }
}
