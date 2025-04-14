namespace StudyConnect.Core.Common;

public class OperationResult<T>
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// The data returned from the operation, if successful.
    /// </summary>
    public T? Data { get; }

    /// <summary>
    /// An error message describing the failure, if applicable.
    /// </summary>
    public string? ErrorMessage { get; }

    private OperationResult(bool isSuccess, T data, string? errorMessage)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
    }

    public static OperationResult<T> Success(T data) => new OperationResult<T>(true, data, null);

    public static OperationResult<T> Failure(string errorMessage) => new OperationResult<T>(false, default!, errorMessage);    
}

public class Result
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
}