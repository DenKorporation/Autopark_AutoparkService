using AutoparkService.Application.Errors.Base;

namespace AutoparkService.Application.Errors.Insurance;

public class InsuranceNotFoundError(string code = "Insurance.NotFound", string message = "Insurance not found")
    : NotFoundError(code, message)
{
    public InsuranceNotFoundError(Guid insuranceId)
        : this(message: $"Insurance {insuranceId} not found")
    {
    }
}
