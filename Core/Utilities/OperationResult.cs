namespace Core.Utilities
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; }
        public string? Message { get; }
        public T? Data { get; }

        private OperationResult(bool isSuccess, T? data, string? message)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
        }

        public static OperationResult<T> Success(T data, string? message = null)
            => new OperationResult<T>(true, data, message);

        public static OperationResult<T> Failure(string message)
            => new OperationResult<T>(false, default, message);
    }
}
