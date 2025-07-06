using FluentValidation;
using TaskTracker.Application.Services.Tasks.Handlers.Commands;

namespace TaskTracker.Application.Services.Tasks.Validators
{
    public class UpdateTaskDateCommandValidator : AbstractValidator<UpdateTaskDateCommand>
    {
        public UpdateTaskDateCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Geçerli bir görev ID'si giriniz");

            RuleFor(x => x.DueDate)
                .Must(dueDate => !dueDate.HasValue || dueDate.Value > DateTime.UtcNow)
                .WithMessage("Bitiş tarihi gelecek bir tarih olmalıdır");
        }
    }
} 