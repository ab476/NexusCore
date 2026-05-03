namespace NC.OperationResults;

public interface IOutcome
{
    string Error { get; }
    bool IsFailure { get; }
    bool IsSuccess { get; }
    string Message { get; }
    int StatusCode { get; }
}

public interface IOutcome<T> : IOutcome
{
    T Value { get; }
}