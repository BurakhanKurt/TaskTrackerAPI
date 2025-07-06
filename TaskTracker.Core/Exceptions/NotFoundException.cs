namespace TaskTracker.Core.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message = "Kayıt bulunamadı") 
            : base(message, 404, "NOT_FOUND")
        {
        }

        public NotFoundException(string message, Exception innerException) 
            : base(message, innerException, 404, "NOT_FOUND")
        {
        }
    }
} 