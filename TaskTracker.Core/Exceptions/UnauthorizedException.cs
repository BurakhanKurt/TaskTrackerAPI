namespace TaskTracker.Core.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message = "Yetkilendirme gerekli") 
            : base(message, 401, "UNAUTHORIZED")
        {
        }

        public UnauthorizedException(string message, Exception innerException) 
            : base(message, innerException, 401, "UNAUTHORIZED")
        {
        }
    }
} 