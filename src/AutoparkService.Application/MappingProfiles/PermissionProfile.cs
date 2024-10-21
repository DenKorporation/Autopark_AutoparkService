using AutoMapper;
using AutoparkService.Application.DTOs.Permission.Request;
using AutoparkService.Application.DTOs.Permission.Response;
using AutoparkService.Domain.Models;

namespace AutoparkService.Application.MappingProfiles;

public class PermissionProfile : Profile
{
    public PermissionProfile()
    {
        CreateMap<PermissionRequest, Permission>()
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => DateOnly.Parse(src.ExpiryDate)))
            .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.VehicleId));

        CreateMap<Permission, PermissionResponse>()
            .ConstructUsing(
                src => new PermissionResponse(
                    src.Id,
                    src.Number,
                    src.ExpiryDate,
                    src.VehicleId,
                    src.ExpiryDate >= DateOnly.FromDateTime(DateTime.Today)));
    }
}
