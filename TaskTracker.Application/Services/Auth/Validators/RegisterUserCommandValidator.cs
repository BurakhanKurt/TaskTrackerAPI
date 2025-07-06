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
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]").WithMessage("Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir");

            RuleFor(x => x.FirstName)
                .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir")
                .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$").WithMessage("Ad sadece harf ve boşluk içerebilir")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir")
                .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$").WithMessage("Soyad sadece harf ve boşluk içerebilir")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^[+]?[0-9\s\-\(\)]+$").WithMessage("Geçerli bir telefon numarası giriniz")
                .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir")
                .MinimumLength(10).WithMessage("Telefon numarası en az 10 karakter olmalıdır")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
        }
    }
} 