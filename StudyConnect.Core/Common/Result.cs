namespace StudyConnect.Core.Common;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Value { get; set; }
    public string? Error { get; set; }
}

public class Result
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
}