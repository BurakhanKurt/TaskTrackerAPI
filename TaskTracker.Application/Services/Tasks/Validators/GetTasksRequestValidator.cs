using FluentValidation;
using TaskTracker.Application.Services.Tasks.DTOs.Request;

namespace TaskTracker.Application.Services.Tasks.Validators
{
    public class GetTasksRequestValidator : AbstractValidator<GetTasksRequest>
    {
        public GetTasksRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Sayfa numarası 1'den büyük olmalıdır")
                .LessThanOrEqualTo(int.MaxValue).WithMessage("Sayfa numarası çok büyük");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Sayfa boyutu 1'den büyük olmalıdır")
                .LessThanOrEqualTo(100).WithMessage("Sayfa boyutu en fazla 100 olabilir");

            RuleFor(x => x.SearchTerm)
                .MaximumLength(100).WithMessage("Arama terimi en fazla 100 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.SearchTerm));
        }
    }
} 