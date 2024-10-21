using AutoMapper;
using AutoparkService.Application.DTOs.MaintenanceRecord.Request;
using AutoparkService.Application.DTOs.MaintenanceRecord.Response;
using AutoparkService.Domain.Models;

namespace AutoparkService.Application.MappingProfiles;

public class MaintenanceRecordProfile : Profile
{
    public MaintenanceRecordProfile()
    {
        CreateMap<MaintenanceRecordRequest, MaintenanceRecord>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateOnly.Parse(src.StartDate)))
            .ForMember(
                dest => dest.EndDate,
                opt => opt.MapFrom(src => src.EndDate != null ? DateOnly.Parse(src.EndDate) : (DateOnly?)null))
            .ForMember(dest => dest.Odometer, opt => opt.MapFrom(src => src.Odometer))
            .ForMember(dest => dest.ServiceCenter, opt => opt.MapFrom(src => src.ServiceCenter))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
            .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.VehicleId));

        CreateMap<MaintenanceRecord, MaintenanceRecordResponse>()
            .ConstructUsing(
                src => new MaintenanceRecordResponse(
                    src.Id,
                    src.Type,
                    src.StartDate,
                    src.EndDate,
                    src.Odometer,
                    src.ServiceCenter,
                    src.Cost,
                    src.VehicleId));
    }
}
