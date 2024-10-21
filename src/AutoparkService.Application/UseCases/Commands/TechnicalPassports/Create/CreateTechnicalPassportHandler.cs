using AutoMapper;
using AutoparkService.Application.DTOs.TechnicalPassport.Request;
using AutoparkService.Application.DTOs.TechnicalPassport.Response;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.TechnicalPassport;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.TechnicalPassports.Create;

public class CreateTechnicalPassportHandler(
    ITechnicalPassportRepository technicalPassportRepository,
    IVehicleRepository vehicleRepository,
    IMapper mapper)
    : ICommandHandler<CreateTechnicalPassportCommand, TechnicalPassportResponse>
{
    public async Task<Result<TechnicalPassportResponse>> Handle(
        CreateTechnicalPassportCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request.Request, cancellationToken);

        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        var createTechnicalPassport = mapper.Map<TechnicalPassport>(request.Request);

        try
        {
            await technicalPassportRepository.CreateAsync(createTechnicalPassport, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("TechnicalPassport.Create");
        }

        return mapper.Map<TechnicalPassportResponse>(createTechnicalPassport);
    }

    private async Task<Result> ValidateRequest(
        TechnicalPassportRequest request,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);

        if (vehicle is null)
        {
            return new VehicleNotFoundError(request.VehicleId);
        }

        var technicalPassportForVehicle =
            await technicalPassportRepository.GetByVehicleId(request.VehicleId, cancellationToken);

        if (technicalPassportForVehicle is not null)
        {
            return new TechnicalPassportDuplicationError(request.VehicleId);
        }

        var technicalPassport = await technicalPassportRepository.GetByNumberAsync(request.Number, cancellationToken);

        if (technicalPassport is not null)
        {
            return new TechnicalPassportDuplicationError(request.Number);
        }

        technicalPassport = await technicalPassportRepository.GetByVinAsync(request.VIN, cancellationToken);

        if (technicalPassport is not null)
        {
            return new TechnicalPassportDuplicationError(request.VIN);
        }

        technicalPassport =
            await technicalPassportRepository.GetByLicensePlateAsync(request.LicensePlate, cancellationToken);

        if (technicalPassport is not null)
        {
            return new TechnicalPassportDuplicationError(request.LicensePlate);
        }

        return Result.Ok();
    }
}
