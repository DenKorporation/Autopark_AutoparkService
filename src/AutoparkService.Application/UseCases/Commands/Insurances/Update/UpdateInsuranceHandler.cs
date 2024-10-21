using AutoMapper;
using AutoparkService.Application.DTOs.Insurance.Response;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Insurance;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.Insurances.Update;

public class UpdateInsuranceHandler(
    IInsuranceRepository insuranceRepository,
    IVehicleRepository vehicleRepository,
    IMapper mapper)
    : ICommandHandler<UpdateInsuranceCommand, InsuranceResponse>
{
    public async Task<Result<InsuranceResponse>> Handle(
        UpdateInsuranceCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);

        if (validationResult.IsFailed)
        {
            return validationResult.ToResult();
        }

        var insurance = validationResult.Value;

        mapper.Map(request.Request, insurance);

        try
        {
            await insuranceRepository.UpdateAsync(insurance, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("Insurance.Update");
        }

        return mapper.Map<InsuranceResponse>(insurance);
    }

    private async Task<Result<Insurance>> ValidateRequest(
        UpdateInsuranceCommand request,
        CancellationToken cancellationToken = default)
    {
        var updateDto = request.Request;

        var insurance = await insuranceRepository.GetByIdAsync(request.Id, cancellationToken);

        if (insurance is null)
        {
            return new InsuranceNotFoundError(request.Id);
        }

        if (insurance.Series != updateDto.Series || insurance.Number != updateDto.Number)
        {
            var duplicateInsurance = await insuranceRepository.GetBySeriesAndNumberAsync(
                updateDto.Series,
                updateDto.Number,
                cancellationToken);

            if (duplicateInsurance is not null)
            {
                return new InsuranceDuplicationError($"{updateDto.Series}{updateDto.Number}");
            }
        }

        if (insurance.VehicleId != updateDto.VehicleId)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(updateDto.VehicleId, cancellationToken);

            if (vehicle is null)
            {
                return new VehicleNotFoundError(updateDto.VehicleId);
            }
        }

        return insurance;
    }
}
