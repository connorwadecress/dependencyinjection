using VehicleApp.Application.Interfaces;

namespace VehicleApp.Infrastructure.Audit;

public class ConsoleAuditService : IAuditService
{
    public void Audit(string action, string detail)
    {
        Console.WriteLine($"[AUDIT] Action: {action} | Detail: {detail} | Time: {DateTime.UtcNow}");
    }
}