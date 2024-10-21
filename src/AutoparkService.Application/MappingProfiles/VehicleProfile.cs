using AutoMapper;
using AutoparkService.Application.DTOs.Vehicle.Request;
using AutoparkService.Application.DTOs.Vehicle.Response;
using AutoparkService.Domain.Models;

namespace AutoparkService.Application.MappingProfiles;

public class VehicleProfile : Profile
{
    public VehicleProfile()
    {
        CreateMap<VehicleRequest, Vehicle>()
            .ForMember(dest => dest.Odometer, opt => opt.MapFrom(src => src.Odometer))
            .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => DateOnly.Parse(src.PurchaseDate)))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Insurances, opt => opt.MapFrom(_ => new List<Insurance>()))
            .ForMember(dest => dest.MaintenanceRecords, opt => opt.MapFrom(src => new List<MaintenanceRecord>()));

        CreateMap<Vehicle, VehicleResponse>()
            .ConstructUsing(
                src => new VehicleResponse(
                    src.Id,
                    src.Odometer,
                    src.PurchaseDate,
                    src.Cost,
                    src.Status,
                    src.UserId,
                    null!,
                    null!,
                    src.Insurances.Select(i => i.Id),
                    src.MaintenanceRecords.Select(mr => mr.Id)))
            .ForMember(
                dest => dest.TechnicalPassportId,
                opt => opt.MapFrom(src => src.TechnicalPassport != null ? src.TechnicalPassport.Id : (Guid?)null))
            .ForMember(
                dest => dest.PermissionId,
                opt => opt.MapFrom(src => src.Permission != null ? src.Permission.Id : (Guid?)null));
    }
}
