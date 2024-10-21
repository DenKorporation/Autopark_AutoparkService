using AutoMapper;
using AutoparkService.Application.DTOs.Insurance.Request;
using AutoparkService.Application.DTOs.Insurance.Response;
using AutoparkService.Domain.Models;

namespace AutoparkService.Application.MappingProfiles;

public class InsuranceProfile : Profile
{
    public InsuranceProfile()
    {
        CreateMap<InsuranceRequest, Insurance>()
            .ForMember(dest => dest.Series, opt => opt.MapFrom(src => src.Series))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType))
            .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.Provider))
            .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => DateOnly.Parse(src.IssueDate)))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateOnly.Parse(src.StartDate)))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateOnly.Parse(src.EndDate)))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
            .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.VehicleId));

        CreateMap<Insurance, InsuranceResponse>()
            .ConstructUsing(
                src => new InsuranceResponse(
                    src.Id,
                    src.Series,
                    src.Number,
                    src.VehicleType,
                    src.Provider,
                    src.IssueDate,
                    src.StartDate,
                    src.EndDate,
                    src.Cost,
                    src.VehicleId,
                    src.EndDate >= DateOnly.FromDateTime(DateTime.Today)));
    }
}
