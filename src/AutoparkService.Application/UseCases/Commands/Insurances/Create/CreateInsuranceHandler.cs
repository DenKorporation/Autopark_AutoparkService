using AutoMapper;
using AutoparkService.Application.DTOs.Insurance.Request;
using AutoparkService.Application.DTOs.Insurance.Response;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Insurance;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.Insurances.Create;

public class CreateInsuranceHandler(
    IInsuranceRepository insuranceRepository,
    IVehicleRepository vehicleRepository,
    IMapper mapper)
    : ICommandHandler<CreateInsuranceCommand, InsuranceResponse>
{
    public async Task<Result<InsuranceResponse>> Handle(
        CreateInsuranceCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request.Request, cancellationToken);

        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        var createInsurance = mapper.Map<Insurance>(request.Request);

        try
        {
            await insuranceRepository.CreateAsync(createInsurance, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("Insurance.Create");
        }

        return mapper.Map<InsuranceResponse>(createInsurance);
    }

    private async Task<Result> ValidateRequest(InsuranceRequest request, CancellationToken cancellationToken = default)
    {
        var vehicle = await vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);

        if (vehicle is null)
        {
            return new VehicleNotFoundError(request.VehicleId);
        }

        var insurance = await insuranceRepository.GetBySeriesAndNumberAsync(
            request.Series,
            request.Number,
            cancellationToken);

        if (insurance is not null)
        {
            return new InsuranceDuplicationError($"{request.Series}{request.Number}");
        }

        return Result.Ok();
    }
}
