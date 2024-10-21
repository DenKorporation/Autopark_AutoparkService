using AutoparkService.Application.DTOs.Common;
using AutoparkService.Application.DTOs.MaintenanceRecord.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Queries.MaintenanceRecords.GetAll;

public record GetAllMaintenanceRecordsQuery(FilterMaintenanceRecordsRequest Request)
    : IQuery<PagedList<MaintenanceRecordResponse>>;
