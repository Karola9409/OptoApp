using System.Runtime.InteropServices.JavaScript;

namespace OptoApi.Models;

public class OperationResult<TData>
{
    private OperationResult(TData data, string message, bool succeeded, ErrorStatus? status)
    {
        Data = data;
        Message = message;
        Succeeded = succeeded;
        Status = status;
    }

    public TData Data { get; }
    public string Message { get; }
    public ErrorStatus? Status { get; }
    public bool Succeeded { get; }

    public static OperationResult<TData> Success(TData data)
    {
        return new OperationResult<TData>(data, "", true, null);
    }

    public static OperationResult<TData> Failure(string message, ErrorStatus status)
    {
        return new OperationResult<TData>(default, message, false, status);
    }
}

public class OperationResult
{
    private OperationResult(string message, bool succeeded, ErrorStatus? status)
    {
        Message = message;
        Succeeded = succeeded;
        Status = status;
    }
    public string Message { get; }
    public ErrorStatus? Status { get; }
    public bool Succeeded { get; }

    public static OperationResult Success()
    {
        return new OperationResult("", true, null);
    }

    public static OperationResult Failure(string message, ErrorStatus status)
    {
        return new OperationResult (message, false, status);
    }
}