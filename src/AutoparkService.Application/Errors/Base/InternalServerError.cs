namespace AutoparkService.Application.Errors.Base;

public class InternalServerError(string code, string message = "Something went wrong")
    : BaseError(code, message)
{
}
