using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.Menu
{
    public class CreateMenuRequestValidator : AbstractValidator<CreateMenuRequest>
    {
        public CreateMenuRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên menu không được để trống.")
                .MaximumLength(100).WithMessage("Tên menu không được vượt quá 100 ký tự.");

            RuleFor(x => x.Url)
                .MaximumLength(200).WithMessage("URL không được vượt quá 200 ký tự.")
                .When(x => !string.IsNullOrEmpty(x.Url));

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Icon)
                .MaximumLength(100).WithMessage("Icon không được vượt quá 100 ký tự.")
                .When(x => !string.IsNullOrEmpty(x.Icon));

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Thứ tự phải lớn hơn hoặc bằng 0.");
        }
    }
}