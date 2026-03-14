namespace TPICAP.Matching.Core.Results;

public class Result<T> : Result
{
    private readonly T? value;

    private Result(T value)
        : base(true, string.Empty)
    {
        this.value = value;
    }

    private Result(string errorMessage)
        : base(false, errorMessage)
    {
    }

    public T Value => IsSuccess
        ? value!
        : throw new InvalidOperationException("A failed result does not contain a value.");

    public static Result<T> Success(T value) => new(value);

    public static new Result<T> Failure(string errorMessage) => new(errorMessage);
}
