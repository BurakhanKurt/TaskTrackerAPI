using FluentValidation;
using TaskTracker.Application.Services.Tasks.Handlers.Queries;

namespace TaskTracker.Application.Services.Tasks.Validators
{
    public class GetTasksQueryValidator : AbstractValidator<GetTasksQuery>
    {
        public GetTasksQueryValidator()
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

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("Başlangıç tarihi bitiş tarihinden büyük olamaz")
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue);

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("Bitiş tarihi başlangıç tarihinden küçük olamaz")
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue);
        }
    }
} 