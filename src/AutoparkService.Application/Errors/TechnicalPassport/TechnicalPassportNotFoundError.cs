using AutoparkService.Application.Errors.Base;

namespace AutoparkService.Application.Errors.TechnicalPassport;

public class TechnicalPassportNotFoundError(
    string code = "TechnicalPassport.NotFound",
    string message = "TechnicalPassport not found")
    : NotFoundError(code, message)
{
    public TechnicalPassportNotFoundError(Guid technicalPassportId)
        : this(message: $"TechnicalPassport '{technicalPassportId}' not found")
    {
    }
}
