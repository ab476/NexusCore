using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NC.OperationResults;

namespace NC.AuthService.Api.Extensions;

public static class ResultExtensions
{
    /// <summary>
    /// Maps a Result to a specific TypedResult based on its StatusCode.
    /// Used for endpoints returning Results with no data.
    /// </summary>
    public static Microsoft.AspNetCore.Http.IResult ToMappedResult(this Outcome result, string? location = null)
    {
        return result.StatusCode switch
        {
            200 => TypedResults.Ok(result),
            201 => TypedResults.Created(location ?? string.Empty, result),
            204 => TypedResults.NoContent(),
            400 => TypedResults.BadRequest(result),
            404 => TypedResults.NotFound(result),
            409 => TypedResults.Conflict(result),
            _ => TypedResults.Json(result, statusCode: result.StatusCode)
        };
    }

    /// <summary>
    /// Maps a generic Result<T> to a specific TypedResult.
    /// </summary>
    public static Microsoft.AspNetCore.Http.IResult ToMappedResult<T>(this Outcome<T> result)
    {
        return result.StatusCode switch
        {
            200 => TypedResults.Ok(result),
            404 => TypedResults.NotFound(result),
            _ => TypedResults.Json(result, statusCode: result.StatusCode)
        };
    }
}