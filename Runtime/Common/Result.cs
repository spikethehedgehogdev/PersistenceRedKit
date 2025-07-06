namespace Klukva.RedKit.Persistence.Common
{
    public readonly struct Result<T>
    {
        public bool Success { get; }
        public T Value { get; }

        private Result(bool success, T value)
        {
            Success = success;
            Value = value;
        }

        public static Result<T> SuccessResult(T value) => new Result<T>(true, value);
        public static Result<T> Fail(T fallback) => new Result<T>(false, fallback);
    }
}