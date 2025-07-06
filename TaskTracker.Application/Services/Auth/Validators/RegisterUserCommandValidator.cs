using FluentValidation;
using TaskTracker.Application.Services.Auth.Handlers.Commands;

namespace TaskTracker.Application.Services.Auth.Validators
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı zorunludur")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır")
                .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("Kullanıcı adı sadece harf, rakam ve alt çizgi içerebilir")
                .Matches("^[a-zA-Z]").WithMessage("Kullanıcı adı harf ile başlamalıdır");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email adresi zorunludur")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz")
                .MaximumLength(100).WithMessage("Email adresi en fazla 100 karakter olabilir")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithMessage("Geçerli bir email formatı giriniz");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre zorunludur")
                .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır")
                .MaximumLength(100).WithMessage("Şifre en fazla 100 karakter olabilir")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&._-])[A-Za-z\d@$!%*?&._-]+").WithMessage("Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir");
        }
    }
} 
