namespace NC.OperationResults;

using System;
using System.Net;

// Base class for operations that don't return data (e.g., Delete, Update)
public class Result : IResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }

    // New properties
    public string Message { get; }
    public int StatusCode { get; }

    // Protected constructor forces the use of the static factory methods
    protected Result(bool isSuccess, string error, string message, HttpStatusCode statusCode)
    {
        // Guard against invalid states
        if (isSuccess && !string.IsNullOrWhiteSpace(error))
            throw new InvalidOperationException("A successful result cannot have an error message.");
        if (!isSuccess && string.IsNullOrWhiteSpace(error))
            throw new InvalidOperationException("A failure result must have an error message.");

        IsSuccess = isSuccess;
        Error = error ?? string.Empty;
        Message = message ?? string.Empty;
        StatusCode = (int)statusCode;
    }

    // Static factory methods for easy creation (with sensible HTTP defaults)
    public static Result Success(string message = "", HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(true, string.Empty, message, statusCode);

    public static Result Failure(string error, string message = "", HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(false, error, message, statusCode);

    // Helpers to easily create generic results
    public static Result<T> Success<T>(T value, string message = "", HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(value, true, string.Empty, message, statusCode);

    public static Result<T> Failure<T>(string error, string message = "", HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(default, false, error, message, statusCode);
}

// Generic class for operations that return data (e.g., Get, Create)
public class Result<T> : Result, IResult<T>
{
    private readonly T? _value;

    // Safely expose the value, throwing an exception if accessed on a failure
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access the value of a failure result.");

    // Internal constructor ensures it's only created via the base Result factory methods
    protected internal Result(T? value, bool isSuccess, string error, string message, HttpStatusCode statusCode)
        : base(isSuccess, error, message, statusCode)
    {
        _value = value;
    }
}