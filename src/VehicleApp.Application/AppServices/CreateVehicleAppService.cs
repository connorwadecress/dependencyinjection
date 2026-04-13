using System;
using System.Collections.Generic;
using System.Text;
using VehicleApp.Domain.Entities;
using VehicleApp.Domain.Interfaces;

namespace VehicleApp.Application.AppServices;

public class CreateVehicleAppService
{
    private readonly IVehicleRepository _repository;

    // CONSTRUCTOR INJECTION - the dependency is required and immutable
    public CreateVehicleAppService(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public Vehicle Execute(Vehicle vehicle)
    {
        vehicle.Id = Guid.NewGuid();
        _repository.Add(vehicle);
        return vehicle;
    }
}
