using System;
using System.Collections.Generic;
using System.Text;
using VehicleApp.Application.Interfaces;
using VehicleApp.Domain.Entities;
using VehicleApp.Domain.Interfaces;

namespace VehicleApp.Application.AppServices
{
    public class LogVehicleAppService
    {
        private readonly IVehicleRepository _repository;

        //constructor injection for required dependency
        public LogVehicleAppService(IVehicleRepository repository)
        {
            _repository = repository;
        }

        //method injection - logger now passed into the call, not stored in the class
        // the caller will then decide which logger to use each time
        public List<Vehicle> GetAllAndLog(IVehicleLogger logger)
        {
            var vehicles = _repository.GetAll();
            logger.Log($"Retrieved {vehicles.Count} vehicles from the repository.");
            return vehicles;
        }

    }
}
