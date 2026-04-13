using Microsoft.AspNetCore.Mvc;
using VehicleApp.Application.AppServices;
using VehicleApp.Application.Interfaces;
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

    private readonly LogVehicleAppService _logService;
    private readonly IVehicleLogger _logger;

    private readonly IAuditService _auditService;

    // CONSTRUCTOR INJECTION — the DI container provides all 4 app services
    public VehicleController(
        CreateVehicleAppService createService,
        GetAllVehiclesAppService getAllService,
        UpdateVehicleAppService updateService,
        DeleteVehicleAppService deleteService,

        LogVehicleAppService logService,
        IVehicleLogger logger,

        IAuditService auditService)
    {
        _createService = createService;
        _getAllService = getAllService;
        _updateService = updateService;
        _deleteService = deleteService;

        _logService = logService;
        _logger = logger;

        // PROPERTY INJECTION — we set the optional dependency on the service after it was constructed
        _createService.AuditService = auditService;
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

    // METHOD INJECTION — the logger is passed into the method, not stored on the service
    [HttpGet("logged")]
    public IActionResult GetAllWithLog()
    {
        var vehicles = _logService.GetAllAndLog(_logger);
        return Ok(vehicles);
    }

}