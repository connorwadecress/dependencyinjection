namespace VehicleApp.Application.Interfaces;

public interface IAuditService
{
    void Audit(string action, string detail);
}