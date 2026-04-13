using System;
using System.Collections.Generic;
using System.Text;

using VehicleApp.Domain;
using VehicleApp.Domain.Interfaces;

namespace VehicleApp.Application.AppServices;

public class DeleteVehicleAppService
{
    private readonly IVehicleRepository _repository;

    public DeleteVehicleAppService(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public void Execute(Guid id)
    {
        _repository.Delete(id);
    }
}
