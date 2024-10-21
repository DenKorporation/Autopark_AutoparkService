using AutoMapper;
using AutoparkService.Application.DTOs.TechnicalPassport.Request;
using AutoparkService.Application.DTOs.TechnicalPassport.Response;
using AutoparkService.Domain.Models;

namespace AutoparkService.Application.MappingProfiles;

public class TechnicalPassportProfile : Profile
{
    public TechnicalPassportProfile()
    {
        CreateMap<TechnicalPassportRequest, TechnicalPassport>()
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.FirstNameLatin, opt => opt.MapFrom(src => src.FirstNameLatin))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.LastNameLatin, opt => opt.MapFrom(src => src.LastNameLatin))
            .ForMember(dest => dest.Patronymic, opt => opt.MapFrom(src => src.Patronymic))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => DateOnly.Parse(src.IssueDate)))
            .ForMember(dest => dest.SAICode, opt => opt.MapFrom(src => src.SAICode))
            .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => src.LicensePlate))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
            .ForMember(dest => dest.CreationYear, opt => opt.MapFrom(src => src.CreationYear))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
            .ForMember(dest => dest.VIN, opt => opt.MapFrom(src => src.VIN))
            .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType))
            .ForMember(dest => dest.MaxWeight, opt => opt.MapFrom(src => src.MaxWeight))
            .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.VehicleId));

        CreateMap<TechnicalPassport, TechnicalPassportResponse>()
            .ConstructUsing(
                src => new TechnicalPassportResponse(
                    src.Id,
                    src.Number,
                    src.FirstName,
                    src.FirstNameLatin,
                    src.LastName,
                    src.LastNameLatin,
                    src.Patronymic,
                    src.Address,
                    src.IssueDate,
                    src.SAICode,
                    src.LicensePlate,
                    src.Brand,
                    src.Model,
                    src.CreationYear,
                    src.Color,
                    src.VIN,
                    src.VehicleType,
                    src.MaxWeight,
                    src.VehicleId));
    }
}
