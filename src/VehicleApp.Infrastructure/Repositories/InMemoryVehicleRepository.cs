using VehicleApp.Domain.Entities;
using VehicleApp.Domain.Interfaces;

namespace VehicleApp.Infrastructure.Repositories;

public class InMemoryVehicleRepository : IVehicleRepository
{
    private readonly List<Vehicle> _vehicles = new List<Vehicle>();

    public Vehicle? GetById(Guid id)
    {
        return _vehicles.FirstOrDefault(v => v.Id == id);
    }

    public List<Vehicle> GetAll()
    {
        return _vehicles;
    }

    public void Add(Vehicle vehicle)
    {
        _vehicles.Add(vehicle);
    }

    public void Update(Vehicle vehicle)
    {
        var existing = _vehicles.FirstOrDefault(v => v.Id == vehicle.Id);
        if (existing != null)
        {
            existing.Make = vehicle.Make;
            existing.Model = vehicle.Model;
            existing.Year = vehicle.Year;
            existing.Colour = vehicle.Colour;
        }
    }

    public void Delete(Guid id)
    {
        var vehicle = _vehicles.FirstOrDefault(v => v.Id == id);
        if (vehicle != null)
            _vehicles.Remove(vehicle);
    }
}