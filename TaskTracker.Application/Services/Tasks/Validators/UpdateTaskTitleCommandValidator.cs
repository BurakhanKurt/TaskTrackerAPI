using FluentValidation;
using TaskTracker.Application.Services.Tasks.Handlers.Commands;

namespace TaskTracker.Application.Services.Tasks.Validators
{
    public class UpdateTaskTitleCommandValidator : AbstractValidator<UpdateTaskTitleCommand>
    {
        public UpdateTaskTitleCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçerli bir görev ID'si giriniz");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık zorunludur")
                .MinimumLength(3).WithMessage("Başlık en az 3 karakter olmalıdır")
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir")
                .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ0-9\s\-_.,!?()]+$").WithMessage("Başlık sadece harf, rakam, boşluk ve özel karakterler içerebilir");
        }
    }
} 