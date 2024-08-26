namespace Ddth.Utilities.Tests.ReflectionHelper;

internal interface IUnusedInterface
{
}

/*----------------------------------------------------------------------*/

internal interface IDummyInterface
{
    string Name { get; }
}

sealed class DummyClass : IDummyInterface
{
    public string Name { get; set; } = "Default";
}

/*----------------------------------------------------------------------*/

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

public interface IDummyService
{
    string Name { get; }
}

public interface IDummyService1 : IDummyService { }

public interface IDummyService2 : IDummyService { }

public class DummyServiceMultiInterfaces : IDummyService1, IDummyService2
{
    public string Name { get; set; }

    public DummyServiceMultiInterfaces(string name)
    {
        Name = name;
    }
}

public class DummyClassNeedIDummyService : IDummyService
{
    public string Name { get; set; }

    public DummyClassNeedIDummyService(IDummyService service)
    {
        Name = service.Name;
    }
}

public class DummyClassNeedIDummyService1 : IDummyService1
{
    public string Name { get; set; }

    public DummyClassNeedIDummyService1(IDummyService1 service)
    {
        Name = service.Name;
    }
}

public class DummyClassNeedIDummyService2 : IDummyService2
{
    public string Name { get; set; }

    public DummyClassNeedIDummyService2(IDummyService2 service)
    {
        Name = service.Name;
    }
}
