namespace TPICAP.Matching.Core.Results;

public class Result
{
    protected Result(bool isSuccess, string errorMessage)
    {
        if (isSuccess && errorMessage.Length > 0)
        {
            throw new ArgumentException("A successful result cannot contain an error message.", nameof(errorMessage));
        }

        if (!isSuccess && errorMessage.Length == 0)
        {
            throw new ArgumentException("A failed result must contain an error message.", nameof(errorMessage));
        }

        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public string ErrorMessage { get; }

    public static Result Success() => new(true, string.Empty);

    public static Result Failure(string errorMessage) => new(false, errorMessage);
}
