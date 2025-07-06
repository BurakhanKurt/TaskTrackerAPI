namespace TaskTracker.Core.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message, string errorCode = "BAD_REQUEST")
            : base(message, 400, errorCode)
        {
        }

        public BadRequestException(string message, Exception innerException, string errorCode = "BAD_REQUEST")
            : base(message, innerException, 400, errorCode)
        {
        }
    }
}
