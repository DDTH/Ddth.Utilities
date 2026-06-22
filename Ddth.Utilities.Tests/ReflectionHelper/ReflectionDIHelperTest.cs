using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ddth.Utilities.Tests.ReflectionHelper;

public class ReflectionDIHelperTest
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEnumerable<object?> _services;

    public ReflectionDIHelperTest()
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

    [Fact]
    public void TestBuildDIParamsWithServiceProvider()
    {
        var parameters = new Type[] { typeof(IDummyService1), typeof(IDummyService2) };
        var result = ReflectionDIHelper.BuildDIParams(_serviceProvider, parameters);
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.IsType<DummyServiceMultiInterfaces>(result[0]);
        Assert.IsAssignableFrom<IDummyService1>(result[0]);
        Assert.IsAssignableFrom<IDummyService2>(result[1]);
        Assert.Equal("1", ((IDummyService1)result[0]!).Name);
        Assert.Equal("2", ((IDummyService2)result[1]!).Name);
    }

    [Fact]
    public void TestBuildDIParamsWithNullServiceProvider()
    {
        var parameters = new Type[] { typeof(IDummyService1), typeof(IDummyService2) };
        var result = ReflectionDIHelper.BuildDIParams(null, _services, parameters);
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.IsAssignableFrom<IDummyService1>(result[0]);
        Assert.IsAssignableFrom<IDummyService2>(result[1]);
        Assert.Equal("0", ((IDummyService1)result[0]!).Name);
        Assert.Equal("0", ((IDummyService2)result[1]!).Name);
    }

    [Fact]
    public void TestBuildDIParamsFallbackToServiceProvider()
    {
        var parameters = new Type[] { typeof(string) };
        var result = ReflectionDIHelper.BuildDIParams(_serviceProvider, _services, parameters);
        Assert.NotNull(result);
        var single = Assert.Single(result);
        Assert.IsType<string>(single);
        Assert.Equal("Dummy String", (string)single!);
    }

    [Fact]
    public void TestBuildDIParamsFallbackToServiceProviderWithNoAdditionalServices()
    {
        var parameters = new Type[] { typeof(string) };
        var result = ReflectionDIHelper.BuildDIParams(_serviceProvider, parameters);
        Assert.NotNull(result);
        var single = Assert.Single(result);
        Assert.IsType<string>(single);
        Assert.Equal("Dummy String", (string)single!);
    }

    [Fact]
    public void TestBuildDIParamsNullResultWithNullServiceProvider()
    {
        var parameters = new Type[] { typeof(IUnusedInterface) };
        var result = ReflectionDIHelper.BuildDIParams(null, _services, parameters);
        Assert.NotNull(result);
        Assert.Null(Assert.Single(result));
    }

    [Fact]
    public void TestBuildDIParamsNullResultWithNullServiceProviderAndNoAdditionalServices()
    {
        var parameters = new Type[] { typeof(IUnusedInterface) };
        var result = ReflectionDIHelper.BuildDIParams(null, null, parameters);
        Assert.NotNull(result);
        Assert.Null(Assert.Single(result));
    }

    [Fact]
    public void TestBuildDIParamsNullResultWithServiceProvider()
    {
        var parameters = new Type[] { typeof(IUnusedInterface) };
        var result = ReflectionDIHelper.BuildDIParams(_serviceProvider, parameters);
        Assert.NotNull(result);
        Assert.Null(Assert.Single(result));
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

    [Fact]
    public void TestBuildDIParamsWithServiceProviderUsingParameterInfo()
    {
        var parameters = typeof(TestClass).GetMethod("TestMethodNeedsDummyService1and2")!.GetParameters();
        var result = ReflectionDIHelper.BuildDIParams(_serviceProvider, parameters);
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.IsAssignableFrom<IDummyService1>(result[0]);
        Assert.IsAssignableFrom<IDummyService2>(result[1]);
        Assert.Equal("1", ((IDummyService1)result[0]!).Name);
        Assert.Equal("2", ((IDummyService2)result[1]!).Name);
    }

    [Fact]
    public void TestBuildDIParamsWithNullServiceProviderUsingParameterInfo()
    {
        var parameters = typeof(TestClass).GetMethod("TestMethodNeedsDummyService1and2")!.GetParameters();
        var result = ReflectionDIHelper.BuildDIParams(null, _services, parameters);
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.IsAssignableFrom<IDummyService1>(result[0]);
        Assert.IsAssignableFrom<IDummyService2>(result[1]);
        Assert.Equal("0", ((IDummyService1)result[0]!).Name);
        Assert.Equal("0", ((IDummyService2)result[1]!).Name);
    }

    [Fact]
    public void TestBuildDIParamsFallbackToServiceProviderUsingParameterInfo()
    {
        var parameters = typeof(TestClass).GetMethod("TestMethodNeedsString")!.GetParameters();
        var result = ReflectionDIHelper.BuildDIParams(_serviceProvider, _services, parameters);
        Assert.NotNull(result);
        var single = Assert.Single(result);
        Assert.IsType<string>(single);
        Assert.Equal("Dummy String", (string)single!);
    }

    [Fact]
    public void TestBuildDIParamsNullResultUsingParameterInfo()
    {
        var parameters = typeof(TestClass).GetMethod("TestMethodNeedsUnusedInterface")!.GetParameters();
        var result = ReflectionDIHelper.BuildDIParams(null, _services, parameters);
        Assert.NotNull(result);
        Assert.Null(Assert.Single(result));
    }

    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestCreateInstanceNotAssignableShouldBeNull()
    {
        var obj = ReflectionDIHelper.CreateInstance<IUnusedInterface>(
            _serviceProvider,
            typeof(DummyClass)
        );
        Assert.Null(obj);
    }

    [Fact]
    public void TestCreateInstanceTypeClass()
    {
        var obj = ReflectionDIHelper.CreateInstance<DummyClass>(
            _serviceProvider,
            typeof(DummyClass)
        );
        Assert.NotNull(obj);
        Assert.Equal("Default", obj.Name);
    }

    [Fact]
    public void TestCreateInstanceTypeInterface()
    {
        var obj = ReflectionDIHelper.CreateInstance<IDummyInterface>(
            _serviceProvider,
            typeof(DummyClass)
        );
        Assert.NotNull(obj);
        Assert.Equal("Default", obj.Name);
    }

    [Fact]
    public void TestCreateInstanceTypeInterfaceFallback()
    {
        var obj = ReflectionDIHelper.CreateInstance<IDummyInterface>(
            _serviceProvider,
            _services,
            typeof(DummyClass)
        );
        Assert.NotNull(obj);
        Assert.Equal("Default", obj.Name);
    }

    /*----------------------------------------------------------------------*/

    [Fact]
    public void TestCreateInstanceWithConstructor()
    {
        var obj = ReflectionDIHelper.CreateInstance<DummyClassWithConstructor>(
            _serviceProvider,
            typeof(DummyClassWithConstructor)
        );
        Assert.NotNull(obj);
        Assert.Equal("Dummy", obj.Name);
    }

    [Fact]
    public void TestCreateInstanceWithSelectedConstructorTypeArr()
    {
        var obj = ReflectionDIHelper.CreateInstance<DummyClassWithMultipleConstructor>(
            _serviceProvider,
            typeof(DummyClassWithMultipleConstructor),
            new Type[] { typeof(string) }
        );
        Assert.NotNull(obj);
        Assert.Equal("constructor 2", obj.Name);
    }

    [Fact]
    public void TestCreateInstanceWithSelectedConstructorParameterInfoArr()
    {
        var pinfo = typeof(DummyClassWithMultipleConstructor).GetConstructors()[0].GetParameters()[0];
        var obj = ReflectionDIHelper.CreateInstance<DummyClassWithMultipleConstructor>(
            _serviceProvider,
            typeof(DummyClassWithMultipleConstructor),
            new ParameterInfo[] { pinfo }
        );
        Assert.NotNull(obj);
        Assert.Equal("constructor 1", obj.Name);
    }

    [Fact]
    public void TestCreateInstanceWithSelectedConstructorTypeArrNotAssignableShouldBeNull()
    {
        var obj = ReflectionDIHelper.CreateInstance<IUnusedInterface>(
            _serviceProvider,
            typeof(DummyClassWithMultipleConstructor),
            new Type[] { typeof(string) }
        );
        Assert.Null(obj);
    }

    [Fact]
    public void TestCreateInstanceWithSelectedConstructorParameterInfoArrNotAssignableShouldBeNull()
    {
        var pinfo = typeof(DummyClassWithMultipleConstructor).GetConstructors()[0].GetParameters()[0];
        var obj = ReflectionDIHelper.CreateInstance<IUnusedInterface>(
            _serviceProvider,
            typeof(DummyClassWithMultipleConstructor),
            new ParameterInfo[] { pinfo }
        );
        Assert.Null(obj);
    }

    [Fact]
    public void TestCreateInstanceNeedIDummyService1()
    {
        var obj = ReflectionDIHelper.CreateInstance<IDummyService1>(
            _serviceProvider,
            typeof(DummyClassNeedIDummyService1)
        );
        Assert.NotNull(obj);
        Assert.Equal("1", obj.Name);
    }

    [Fact]
    public void TestCreateInstanceNeedIDummyService2()
    {
        var obj = ReflectionDIHelper.CreateInstance<IDummyService2>(
            _serviceProvider,
            typeof(DummyClassNeedIDummyService2)
        );
        Assert.NotNull(obj);
        Assert.Equal("2", obj.Name);
    }

    [Fact]
    public void TestCreateInstanceOfDummyService1HasIDummyService0()
    {
        var obj = ReflectionDIHelper.CreateInstance<IDummyService>(
            _serviceProvider,
            _services,
            typeof(DummyClassNeedIDummyService1)
        );
        Assert.NotNull(obj);
        Assert.Equal("0", obj.Name);
    }

    [Fact]
    public void TestCreateInstanceOfDummyService2HasIDummyService0()
    {
        var obj = ReflectionDIHelper.CreateInstance<IDummyService>(
            _serviceProvider,
            _services,
            typeof(DummyClassNeedIDummyService2)
        );
        Assert.NotNull(obj);
        Assert.Equal("0", obj.Name);
    }
}
