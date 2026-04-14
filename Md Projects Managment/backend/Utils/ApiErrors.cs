using Microsoft.AspNetCore.Mvc;

namespace Backend.Utils;

public static class ApiErrors
{
    public static ProblemDetails NotFound(string detail)
    {
        return new ProblemDetails
        {
            Title = "Not Found",
            Status = StatusCodes.Status404NotFound,
            Detail = detail
        };
    }

    public static ProblemDetails BadRequest(string detail)
    {
        return new ProblemDetails
        {
            Title = "Bad Request",
            Status = StatusCodes.Status400BadRequest,
            Detail = detail
        };
    }
}
