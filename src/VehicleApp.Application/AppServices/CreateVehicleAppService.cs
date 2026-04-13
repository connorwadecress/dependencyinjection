using VehicleApp.Application.Interfaces;
using VehicleApp.Domain.Entities;
using VehicleApp.Domain.Interfaces;

namespace VehicleApp.Application.AppServices;

public class CreateVehicleAppService
{
    private readonly IVehicleRepository _repository;

    // PROPERTY INJECTION — optional dependency, set after construction
    // If nothing sets this, it stays null and the service still works fine
    public IAuditService? AuditService { get; set; }

    // Constructor injection for the required dependency
    public CreateVehicleAppService(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public Vehicle Execute(Vehicle vehicle)
    {
        vehicle.Id = Guid.NewGuid();
        _repository.Add(vehicle);

        // Only audits if AuditService was provided — otherwise skips silently
        AuditService?.Audit("CreateVehicle", $"Created vehicle {vehicle.Make} {vehicle.Model} with Id {vehicle.Id}");

        return vehicle;
    }
}