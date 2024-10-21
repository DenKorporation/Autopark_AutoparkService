using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Infrastructure.Contexts;

namespace AutoparkService.Infrastructure.Repositories.Implementations;

public class MaintenanceRecordRepository(AutoParkDbContext dbContext)
    : Repository<MaintenanceRecord>(dbContext), IMaintenanceRecordRepository
{
}
