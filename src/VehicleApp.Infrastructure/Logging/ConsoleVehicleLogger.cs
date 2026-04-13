using System;
using System.Collections.Generic;
using System.Text;
using VehicleApp.Application.Interfaces;

namespace VehicleApp.Infrastructure.Logging
{
    public class ConsoleVehicleLogger : IVehicleLogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[VehicleLog] {DateTime.UtcNow:HH:mm:ss} - {message}");
        }
    }
}
