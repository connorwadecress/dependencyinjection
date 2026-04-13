using VehicleApp.Application.AppServices;
using VehicleApp.Application.Interfaces;
using VehicleApp.Domain.Interfaces;
using VehicleApp.Infrastructure.Logging;
using VehicleApp.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// --- DI REGISTRATIONS (Composition Root) ---

// SINGLETON — one shared instance for the app's lifetime
// The in-memory list lives inside this instance, so data persists across requests
builder.Services.AddSingleton<IVehicleRepository, InMemoryVehicleRepository>();

// SCOPED — one instance per HTTP request
// App services are stateless (they hold no data), so scoped is the right choice here
builder.Services.AddScoped<CreateVehicleAppService>();
builder.Services.AddScoped<GetAllVehiclesAppService>();
builder.Services.AddScoped<UpdateVehicleAppService>();
builder.Services.AddScoped<DeleteVehicleAppService>();
builder.Services.AddTransient<IVehicleLogger, ConsoleVehicleLogger>();
builder.Services.AddScoped<LogVehicleAppService>();


// -------------------------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();