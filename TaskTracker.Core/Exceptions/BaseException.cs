namespace TaskTracker.Core.Exceptions
{
    public abstract class BaseException : Exception
    {
        public int StatusCode { get; }
        public string ErrorCode { get; }

        protected BaseException(string message, int statusCode = 500, string errorCode = "INTERNAL_ERROR") 
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }

        protected BaseException(string message, Exception innerException, int statusCode = 500, string errorCode = "INTERNAL_ERROR") 
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
} 