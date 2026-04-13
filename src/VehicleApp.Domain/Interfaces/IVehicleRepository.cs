using System;
using System.Collections.Generic;
using System.Text;
using VehicleApp.Domain.Entities;

namespace VehicleApp.Domain.Interfaces;

public interface IVehicleRepository
{
    Vehicle? GetById(Guid id);
    List<Vehicle> GetAll();
    void Add(Vehicle vehicle);
    void Update(Vehicle vehicle);
    void Delete(Guid id);
}