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
    // This is how a DI container can auto-discover implementations
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
    // This is exactly what the DI container reads to know what to inject
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
    // Shows how the container could discover property injection candidates
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
    // No DI container involved — we do it ourselves step by step
    [HttpGet("simulate-resolution")]
    public IActionResult SimulateResolution()
    {
        // Step 1 — find the constructor
        var serviceType = typeof(CreateVehicleAppService);
        var constructor = serviceType.GetConstructors().First();

        // Step 2 — create the dependency the constructor needs
        var repository = new InMemoryVehicleRepository();

        // Step 3 — invoke the constructor with the dependency (this is what Invoke does)
        var service = (CreateVehicleAppService)constructor.Invoke(new object[] { repository });

        // Step 4 — find the property and set it (simulates property injection)
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
