using Microsoft.AspNetCore.Mvc;
using VehicleApp.Application.AppServices;
using VehicleApp.Domain.Interfaces;
using VehicleApp.Infrastructure.Audit;
using VehicleApp.Infrastructure.Repositories;

namespace VehicleApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReflectionDemoController : ControllerBase
{
    // Scan an assembly to find all types that implement IVehicleRepository
    // auto discover impelemtnation (like how DI does it)
    [HttpGet("implementations")]
    public IActionResult FindImplementations()
    {
        var interfaceType = typeof(IVehicleRepository);
        var assembly = typeof(InMemoryVehicleRepository).Assembly;

        var implementations = assembly.GetTypes()
            .Where(t => interfaceType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => new { t.Name, t.FullName })
            .ToList();

        return Ok(new
        {
            Scanning = "VehicleApp.Infrastructure assembly",
            Interface = interfaceType.Name,
            ImplementationsFound = implementations
        });
    }

    // Inspect the constructor of CreateVehicleAppService
    // constructors and paramters (needed for DI)
    [HttpGet("constructor")]
    public IActionResult InspectConstructor()
    {
        var type = typeof(CreateVehicleAppService);
        var constructor = type.GetConstructors().First();
        var parameters = constructor.GetParameters()
            .Select(p => new { p.Name, Type = p.ParameterType.Name })
            .ToList();

        return Ok(new
        {
            Class = type.Name,
            ConstructorParameters = parameters,
            Message = "The DI container calls GetConstructors() and GetParameters() to discover what to inject"
        });
    }

    // Inspect injectable properties on CreateVehicleAppService
    // how container discovers property inkection for AuditService
    [HttpGet("properties")]
    public IActionResult InspectProperties()
    {
        var type = typeof(CreateVehicleAppService);
        var injectableProperties = type.GetProperties()
            .Where(p => p.PropertyType.IsInterface && p.CanWrite)
            .Select(p => new { p.Name, Type = p.PropertyType.Name })
            .ToList();

        return Ok(new
        {
            Class = type.Name,
            InjectableProperties = injectableProperties,
            Message = "Writable interface-typed properties are candidates for property injection"
        });
    }

    // Manually simulate what the DI container does using reflection
    // this is if we didnt use program.cs to do DI container
    [HttpGet("simulate-resolution")]
    public IActionResult SimulateResolution()
    {
        //find the constructor
        var serviceType = typeof(CreateVehicleAppService);
        var constructor = serviceType.GetConstructors().First();

        //create the dependency the constructor needs
        var repository = new InMemoryVehicleRepository();

        //invoke the constructor with the dependency
        var service = (CreateVehicleAppService)constructor.Invoke(new object[] { repository });

        //find the property and set it - this is property injection
        var auditProperty = serviceType.GetProperty(nameof(CreateVehicleAppService.AuditService));
        auditProperty?.SetValue(service, new ConsoleAuditService());

        return Ok(new
        {
            Message = "Manually resolved CreateVehicleAppService using reflection — no DI container",
            Steps = new[]
            {
                "1. GetConstructors() — found the constructor",
                "2. GetParameters() — saw it needs IVehicleRepository",
                "3. Created InMemoryVehicleRepository manually",
                "4. constructor.Invoke() — called the constructor with the dependency",
                "5. GetProperty() — found the AuditService property",
                "6. SetValue() — set the property (property injection via reflection)"
            }
        });
    }
}
