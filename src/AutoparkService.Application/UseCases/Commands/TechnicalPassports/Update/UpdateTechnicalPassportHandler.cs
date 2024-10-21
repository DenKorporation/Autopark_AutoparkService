using AutoMapper;
using AutoparkService.Application.DTOs.TechnicalPassport.Response;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.TechnicalPassport;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.TechnicalPassports.Update;

public class UpdateTechnicalPassportHandler(
    ITechnicalPassportRepository technicalPassportRepository,
    IVehicleRepository vehicleRepository,
    IMapper mapper)
    : ICommandHandler<UpdateTechnicalPassportCommand, TechnicalPassportResponse>
{
    public async Task<Result<TechnicalPassportResponse>> Handle(
        UpdateTechnicalPassportCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);

        if (validationResult.IsFailed)
        {
            return validationResult.ToResult();
        }

        var technicalPassport = validationResult.Value;

        mapper.Map(request.Request, technicalPassport);

        try
        {
            await technicalPassportRepository.UpdateAsync(technicalPassport, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("TechnicalPassport.Update");
        }

        return mapper.Map<TechnicalPassportResponse>(technicalPassport);
    }

    private async Task<Result<TechnicalPassport>> ValidateRequest(
        UpdateTechnicalPassportCommand request,
        CancellationToken cancellationToken = default)
    {
        var updateDto = request.Request;

        var technicalPassport = await technicalPassportRepository.GetByIdAsync(request.Id, cancellationToken);

        if (technicalPassport is null)
        {
            return new TechnicalPassportNotFoundError(request.Id);
        }

        if (technicalPassport.Number != updateDto.Number)
        {
            var duplicateTechnicalPassport =
                await technicalPassportRepository.GetByNumberAsync(updateDto.Number, cancellationToken);

            if (duplicateTechnicalPassport is not null)
            {
                return new TechnicalPassportDuplicationError(updateDto.Number);
            }
        }

        if (technicalPassport.VIN != updateDto.VIN)
        {
            var duplicateTechnicalPassport =
                await technicalPassportRepository.GetByVinAsync(updateDto.VIN, cancellationToken);

            if (duplicateTechnicalPassport is not null)
            {
                return new TechnicalPassportDuplicationError(updateDto.VIN);
            }
        }

        if (technicalPassport.LicensePlate != updateDto.LicensePlate)
        {
            var duplicateTechnicalPassport =
                await technicalPassportRepository.GetByLicensePlateAsync(updateDto.LicensePlate, cancellationToken);

            if (duplicateTechnicalPassport is not null)
            {
                return new TechnicalPassportDuplicationError(updateDto.LicensePlate);
            }
        }

        if (technicalPassport.VehicleId != updateDto.VehicleId)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(updateDto.VehicleId, cancellationToken);

            if (vehicle is null)
            {
                return new VehicleNotFoundError(updateDto.VehicleId);
            }

            var technicalPassportForVehicle =
                await technicalPassportRepository.GetByVehicleId(updateDto.VehicleId, cancellationToken);

            if (technicalPassportForVehicle is not null)
            {
                return new TechnicalPassportDuplicationError(updateDto.VehicleId);
            }
        }

        return technicalPassport;
    }
}
