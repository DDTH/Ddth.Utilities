using System.Reflection;

namespace Ddth.Utilities;

/// <summary>
/// Reflection helper class with Dependency Injection support.
/// </summary>
public static class ReflectionDIHelper
{
    /// <summary>
    /// Convenience method to build DI parameters for a contructor or method.
    /// </summary>
    /// <param name="serviceProvider">the service provider to look up for services</param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static object?[] BuildDIParams(IServiceProvider serviceProvider, ParameterInfo[] parameters)
    {
        return BuildDIParams(serviceProvider, null, parameters);
    }

    /// <summary>
    /// Convenience method to build DI parameters for a contructor or method.
    /// </summary>
    /// <param name="serviceProvider">the service provider to look up for services</param>
    /// <param name="services">additional services to look up</param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    /// <remarks>
    ///     services takes precedence over serviceProvider if both are provided.
    /// </remarks>
    public static object?[] BuildDIParams(IServiceProvider? serviceProvider, IEnumerable<object?>? services, ParameterInfo[] parameters)
    {
        return parameters.Select(param =>
        {
            var service = services?.SingleOrDefault(s => s?.GetType().IsAssignableTo(param.ParameterType) ?? false);
            return service ?? serviceProvider?.GetService(param.ParameterType);
        }).ToArray();
    }

    /// <summary>
    /// Convenience method to build DI parameters for a contructor or method.
    /// </summary>
    /// <param name="serviceProvider">the service provider to look up for services</param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static object?[] BuildDIParams(IServiceProvider serviceProvider, Type[] parameters)
    {
        return BuildDIParams(serviceProvider, null, parameters);
    }

    /// <summary>
    /// Convenience method to build DI parameters for a contructor or method.
    /// </summary>
    /// <param name="serviceProvider">the service provider to look up for services</param>
    /// <param name="services">additional services to look up</param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    /// <remarks>
    ///     services takes precedence over serviceProvider if both are provided.
    /// </remarks>
    public static object?[] BuildDIParams(IServiceProvider? serviceProvider, IEnumerable<object?>? services, Type[] parameters)
    {
        return parameters.Select(param =>
        {
            var service = services?.SingleOrDefault(s => s?.GetType().IsAssignableTo(param) ?? false);
            return service ?? serviceProvider?.GetService(param);
        }).ToArray();
    }

    /// <summary>
    /// Convenience method to create an instance of a class with constructor-based DI.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceProvider">the service provider to look up for services</param>
    /// <param name="objType"></param>
    /// <returns></returns>
    /// <remarks>
    ///     null is returned if objType is not assignable to T.<br/>
    ///     The first available constructor is used to create the instance. If the class has multiple constructors, which one is chosen is not guaranteed.
    /// </remarks>
    public static T? CreateInstance<T>(IServiceProvider serviceProvider, Type objType)
    {
        return CreateInstance<T>(serviceProvider, null, objType);
    }

    /// <summary>
    /// Convenience method to create an instance of a class with constructor-based DI.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceProvider">the service provider to look up for services</param>
    /// <param name="services">additional services to look up</param>
    /// <param name="objType"></param>
    /// <returns></returns>
    /// <remarks>
    ///     null is returned if objType is not assignable to T.<br/>
    ///     services takes precedence over serviceProvider if both are provided.<br/>
    ///     The first available constructor is used to create the instance. If the class has multiple constructors, which one is chosen is not guaranteed.
    /// </remarks>
    public static T? CreateInstance<T>(IServiceProvider? serviceProvider, IEnumerable<object?>? services, Type objType)
    {
        if (!objType.IsAssignableTo(typeof(T))) return default;
        var constructor = objType.GetConstructors().First();
        var constructorArgs = BuildDIParams(serviceProvider, services, constructor.GetParameters());
        return (T?)Activator.CreateInstance(objType, constructorArgs);
    }

    /// <summary>
    /// Convenience method to create an instance of a class with constructor-based DI.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceProvider">the service provider to look up for services</param>
    /// <param name="objType"></param>
    /// <param name="constructorParams"></param>
    /// <returns></returns>
    /// <remarks>
    ///     null is returned if objType is not assignable to T.<br/>
    ///     The first constructor whose parameters match the constructorParams is used to create the instance.
    /// </remarks>
    public static T? CreateInstance<T>(IServiceProvider serviceProvider, Type objType, ParameterInfo[] constructorParams)
    {
        return CreateInstance<T>(serviceProvider, null, objType, constructorParams);
    }

    /// <summary>
    /// Convenience method to create an instance of a class with constructor-based DI.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceProvider">the service provider to look up for services</param>
    /// <param name="services">additional services to look up</param>
    /// <param name="objType"></param>
    /// <param name="constructorParams"></param>
    /// <returns></returns>
    /// <remarks>
    ///     null is returned if objType is not assignable to T.<br/>
    ///     services takes precedence over serviceProvider if both are provided.<br/>
    ///     The first constructor whose parameters match the constructorParams is used to create the instance.
    /// </remarks>
    public static T? CreateInstance<T>(IServiceProvider? serviceProvider, IEnumerable<object?>? services, Type objType, ParameterInfo[] constructorParams)
    {
        if (!objType.IsAssignableTo(typeof(T))) return default;
        var constructorArgs = BuildDIParams(serviceProvider, services, constructorParams);
        return (T?)Activator.CreateInstance(objType, constructorArgs);
    }

    /// <summary>
    /// Convenience method to create an instance of a class with constructor-based DI.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceProvider">the service provider to look up for services</param>
    /// <param name="objType"></param>
    /// <param name="constructorParams"></param>
    /// <returns></returns>
    /// <remarks>
    ///     null is returned if objType is not assignable to T.<br/>
    ///     The first constructor whose parameters match the constructorParams is used to create the instance.
    /// </remarks>
    public static T? CreateInstance<T>(IServiceProvider serviceProvider, Type objType, Type[] constructorParams)
    {
        return CreateInstance<T>(serviceProvider, null, objType, constructorParams);
    }

    /// <summary>
    /// Convenience method to create an instance of a class with constructor-based DI.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceProvider">the service provider to look up for services</param>
    /// <param name="services">additional services to look up</param>
    /// <param name="objType"></param>
    /// <param name="constructorParams"></param>
    /// <returns></returns>
    /// <remarks>
    ///     null is returned if objType is not assignable to T.<br/>
    ///     services takes precedence over serviceProvider if both are provided.<br/>
    ///     The first constructor whose parameters match the constructorParams is used to create the instance.
    /// </remarks>
    public static T? CreateInstance<T>(IServiceProvider? serviceProvider, IEnumerable<object?>? services, Type objType, Type[] constructorParams)
    {
        if (!objType.IsAssignableTo(typeof(T))) return default;
        var constructorArgs = BuildDIParams(serviceProvider, services, constructorParams);
        return (T?)Activator.CreateInstance(objType, constructorArgs);
    }
}
