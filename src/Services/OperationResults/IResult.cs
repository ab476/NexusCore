namespace NC.OperationResults;

public interface IResult
{
    string Error { get; }
    bool IsFailure { get; }
    bool IsSuccess { get; }
    string Message { get; }
    int StatusCode { get; }
}

public interface IResult<T> : IResult
{
    T Value { get; }
}