using FluentValidation;
using TaskTracker.Application.Services.Auth.Handlers.Commands;

namespace TaskTracker.Application.Services.Auth.Validators
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email adresi zorunludur")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz")
                .MaximumLength(100).WithMessage("Email adresi en fazla 100 karakter olabilir")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithMessage("Geçerli bir email formatı giriniz");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre zorunludur")
                .MinimumLength(1).WithMessage("Şifre boş olamaz")
                .MaximumLength(100).WithMessage("Şifre en fazla 100 karakter olabilir");
        }
    }
} 