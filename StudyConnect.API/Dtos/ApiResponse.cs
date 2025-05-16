namespace StudyConnect.API.Dtos;

/// <summary>
/// A generic wrapper for API responses, containing either data or an error message.
/// </summary>
/// <typeparam name="T">The type of the data being returned in the response.</typeparam>
public class ApiResponse<T>
{

        /// <summary>
    /// The data returned by the API.
    /// </summary>
    public T Data { get; set; }

        /// <summary>
    /// The error message, if an error occurred.
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Creates a successful response containing the specified data.
    /// </summary>
    /// <param name="data">The data to include in the response.</param>
    public ApiResponse(T data)
    {
        Data = data;
        Error = null;
    }

    
    /// <summary>
    /// Creates a failed response containing the specified error message.
    /// </summary>
    /// <param name="error">The error message describing what went wrong.</param>
    public ApiResponse(string error)
    {
        Data = default!;
        Error = error;
    }
}

