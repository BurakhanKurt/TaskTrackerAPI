using FluentValidation;
using TaskTracker.Application.Services.Tasks.DTOs.Request;

namespace TaskTracker.Application.Services.Tasks.Validators
{
    public class UpdateTaskStatusRequestValidator : AbstractValidator<UpdateTaskStatusRequest>
    {
        public UpdateTaskStatusRequestValidator()
        {
            RuleFor(x => x.IsCompleted)
                .NotNull().WithMessage("Tamamlanma durumu belirtilmelidir");
        }
    }
} 