namespace Core.Entities;

public class OperationResult<T>
    where T : class
{
    public bool IsSuccessful { get; set; }
    public string ErrorMessage { get; set; } = String.Empty;
    public T? Data { get; set; }

    public static OperationResult<T> Success(T data) =>
        new OperationResult<T> { IsSuccessful = true, Data = data };

    public static OperationResult<T> Failure(string errorMessage = "something went wrong") =>
        new OperationResult<T> { IsSuccessful = false, ErrorMessage = errorMessage };
}

public class OperationResult
{
    public bool IsSuccessful { get; set; }
    public string ErrorMessage { get; set; } = String.Empty;


    public static OperationResult Success() =>
        new OperationResult{ IsSuccessful = true, };

    public static OperationResult Failure(string errorMessage = "something went wrong") =>
        new OperationResult { IsSuccessful = false, ErrorMessage = errorMessage };
}