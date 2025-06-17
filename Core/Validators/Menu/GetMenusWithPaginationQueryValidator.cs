using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.Menu
{
    public class GetMenusWithPaginationQueryValidator : AbstractValidator<GetMenusWithPaginationQuery>
    {
        public GetMenusWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .NotNull().WithMessage("Số trang không được để trống.");

            RuleFor(x => x.PageSize)
                .NotNull().WithMessage("Kích thước trang không được để trống.");

            RuleFor(x => x.Keywords)
                .MaximumLength(200).WithMessage("Từ khóa tìm kiếm không được vượt quá 200 ký tự.")
                .When(x => !string.IsNullOrWhiteSpace(x.Keywords));

        }
    }
}