using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleApp.Domain.Entities;
public class Vehicle
{
    public Guid Id { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Colour { get; set; } = string.Empty;
}

