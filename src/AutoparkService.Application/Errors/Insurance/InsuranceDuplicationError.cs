using AutoparkService.Application.Errors.Base;

namespace AutoparkService.Application.Errors.Insurance;

public class InsuranceDuplicationError(
    string code = "Insurance.Duplication",
    string message = "Insurance already exist")
    : ConflictError(code, message)
{
    public InsuranceDuplicationError(string seriesAndNumber)
        : this(message: $"Insurance '{seriesAndNumber}' already exist")
    {
    }
}
