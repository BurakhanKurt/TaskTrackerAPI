using FluentValidation;
using TaskTracker.Application.Services.Tasks.Handlers.Commands;

namespace TaskTracker.Application.Services.Tasks.Validators
{
    public class UpdateTaskStatusCommandValidator : AbstractValidator<UpdateTaskStatusCommand>
    {
        public UpdateTaskStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçerli bir görev ID'si giriniz");

            RuleFor(x => x.IsCompleted)
                .NotNull().WithMessage("Tamamlanma durumu belirtilmelidir");
        }
    }
} 