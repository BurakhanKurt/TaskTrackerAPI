using FluentValidation;
using TaskTracker.Application.Services.Auth.Handlers.Commands;

namespace TaskTracker.Application.Services.Auth.Validators
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı zorunludur")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır")
                .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre zorunludur")
                .MinimumLength(1).WithMessage("Şifre boş olamaz")
                .MaximumLength(100).WithMessage("Şifre en fazla 100 karakter olabilir");
        }
    }
} 