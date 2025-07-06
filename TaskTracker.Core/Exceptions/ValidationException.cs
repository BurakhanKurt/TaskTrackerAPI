using FluentValidation.Results;

namespace TaskTracker.Core.Exceptions
{
    public class ValidationException : BaseException
    {
        public ValidationResult ValidationResult { get; }

        public ValidationException(string message, ValidationResult validationResult) 
            : base(message, 400, "VALIDATION_ERROR")
        {
            ValidationResult = validationResult;
        }

        public ValidationException(ValidationResult validationResult) 
            : base("Validation failed", 400, "VALIDATION_ERROR")
        {
            ValidationResult = validationResult;
        }
    }
} 