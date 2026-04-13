using Microsoft.AspNetCore.Mvc;
using VehicleApp.Application.AppServices;
using VehicleApp.Domain.Entities;

namespace VehicleApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly CreateVehicleAppService _createService;
    private readonly GetAllVehiclesAppService _getAllService;
    private readonly UpdateVehicleAppService _updateService;
    private readonly DeleteVehicleAppService _deleteService;

    // CONSTRUCTOR INJECTION — the DI container provides all 4 app services
    public VehicleController(
        CreateVehicleAppService createService,
        GetAllVehiclesAppService getAllService,
        UpdateVehicleAppService updateService,
        DeleteVehicleAppService deleteService)
    {
        _createService = createService;
        _getAllService = getAllService;
        _updateService = updateService;
        _deleteService = deleteService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] Vehicle vehicle)
    {
        var created = _createService.Execute(vehicle);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var vehicles = _getAllService.Execute();
        return Ok(vehicles);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] Vehicle vehicle)
    {
        vehicle.Id = id;
        _updateService.Execute(vehicle);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _deleteService.Execute(id);
        return NoContent();
    }
}