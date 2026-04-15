using VehicleApp.Application.AppServices;
using VehicleApp.Application.Interfaces;
using VehicleApp.Domain.Interfaces;
using VehicleApp.Infrastructure.Logging;
using VehicleApp.Infrastructure.Repositories;
using VehicleApp.Infrastructure.Audit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- DI REGISTRATIONS (Composition Root) ---

// SINGLETON � one shared instance for the app's lifetime
// The in-memory list lives inside this instance, so data persists across requests
builder.Services.AddSingleton<IVehicleRepository, InMemoryVehicleRepository>();

// SCOPED � one instance per HTTP request
// App services are stateless (they hold no data), so scoped is the right choice here
builder.Services.AddScoped<CreateVehicleAppService>();
builder.Services.AddScoped<GetAllVehiclesAppService>();
builder.Services.AddScoped<UpdateVehicleAppService>();
builder.Services.AddScoped<DeleteVehicleAppService>();
builder.Services.AddScoped<LogVehicleAppService>();

// TRANSIENT � a new instance every time it's requested 
// For simple logging and auditing, we don't need to maintain any state, so transient is fine
builder.Services.AddTransient<IAuditService, ConsoleAuditService>();
builder.Services.AddTransient<IVehicleLogger, ConsoleVehicleLogger>();

// -------------------------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();