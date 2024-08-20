using System.Reflection;

namespace Ddth.Utilities;

/// <summary>
/// Reflection helper class with Dependency Injection support.
/// </summary>
public static class ReflectionDIHelper
{
    /// <summary>
    /// Convenience method to create an instance of a class with constructor-based DI.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <param name="objType"></param>
    /// <returns></returns>
    /// <remarks>The first available constructor is used to create the instance. If the class has multiple constructors, which one is chosen is not guaranteed.</remarks>
    public static T? CreateInstance<T>(IServiceProvider serviceProvider, Type objType)
    {
        var constructor = objType.GetConstructors().First();
        var constructorArgs = BuildDIParams(serviceProvider, constructor.GetParameters());
        return (T?)Activator.CreateInstance(objType, constructorArgs);
    }

    /// <summary>
    /// Convenience method to create an instance of a class with constructor-based DI.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <param name="objType"></param>
    /// <param name="constructorParams"></param>
    /// <returns></returns>
    /// <remarks>The first constructor whose parameters match the constructorParams is used to create the instance.</remarks>
    public static T? CreateInstance<T>(IServiceProvider serviceProvider, Type objType, ParameterInfo[] constructorParams)
    {
        var constructorArgs = BuildDIParams(serviceProvider, constructorParams);
        return (T?)Activator.CreateInstance(objType, constructorArgs);
    }

    /// <summary>
    /// Convenience method to create an instance of a class with constructor-based DI.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <param name="objType"></param>
    /// <param name="constructorParams"></param>
    /// <returns></returns>
    /// <remarks>The first constructor whose parameters match the constructorParams is used to create the instance.</remarks>
    public static T? CreateInstance<T>(IServiceProvider serviceProvider, Type objType, Type[] constructorParams)
    {
        var constructorArgs = BuildDIParams(serviceProvider, constructorParams);
        return (T?)Activator.CreateInstance(objType, constructorArgs);
    }

    private static object?[] BuildDIParams(IServiceProvider serviceProvider, ParameterInfo[] constructorParams)
    {
        return constructorParams.Select(param => serviceProvider.GetService(param.ParameterType)).ToArray();
    }

    private static object?[] BuildDIParams(IServiceProvider serviceProvider, Type[] constructorParams)
    {
        return constructorParams.Select(param => serviceProvider.GetService(param)).ToArray();
    }
}
