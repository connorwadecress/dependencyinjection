using VehicleApp.Domain;
using VehicleApp.Domain.Entities;
using VehicleApp.Domain.Interfaces;

namespace VehicleApp.Application.AppServices;

public class UpdateVehicleAppService
{
    private readonly IVehicleRepository _repository;

    public UpdateVehicleAppService(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public void Execute(Vehicle vehicle)
    {
        _repository.Update(vehicle);
    }
}