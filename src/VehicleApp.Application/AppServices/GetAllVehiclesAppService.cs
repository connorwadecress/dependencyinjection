using VehicleApp.Domain;
using VehicleApp.Domain.Entities;
using VehicleApp.Domain.Interfaces;

namespace VehicleApp.Application.AppServices;

public class GetAllVehiclesAppService
{
    private readonly IVehicleRepository _repository;

    public GetAllVehiclesAppService(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public List<Vehicle> Execute()
    {
        return _repository.GetAll();
    }
}