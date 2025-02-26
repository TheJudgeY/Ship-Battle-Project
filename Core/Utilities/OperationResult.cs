namespace Core.Utilities
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }
        public T? Data { get; }

        private OperationResult(bool isSuccess, T? data, string? errorMessage)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static OperationResult<T> Success(T data) => new OperationResult<T>(true, data, null);

        public static OperationResult<T> Failure(string errorMessage) => new OperationResult<T>(false, default, errorMessage);
    }
}
