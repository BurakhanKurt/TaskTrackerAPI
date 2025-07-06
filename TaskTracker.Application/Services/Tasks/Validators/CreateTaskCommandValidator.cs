using FluentValidation;
using TaskTracker.Application.Services.Tasks.Handlers.Commands;

namespace TaskTracker.Application.Services.Tasks.Validators
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık zorunludur")
                .MinimumLength(3).WithMessage("Başlık en az 3 karakter olmalıdır")
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir")
                .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ0-9\s\-_.,!?()]+$").WithMessage("Başlık sadece harf, rakam, boşluk ve özel karakterler içerebilir");

            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Bitiş tarihi bugün veya daha sonraki bir tarih olmalıdır")
                .When(x => x.DueDate.HasValue);
        }
    }
} 