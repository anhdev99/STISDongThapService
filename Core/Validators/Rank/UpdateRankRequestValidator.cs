using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.Rank
{
    public class UpdateRankRequestValidator : AbstractValidator<UpdateRankRequest>
    {
        public UpdateRankRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id không hợp lệ.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Mã cấp bậc không được để trống.")
                .MaximumLength(50).WithMessage("Mã cấp bậc không được vượt quá 50 ký tự.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên cấp bậc không được để trống.")
                .MaximumLength(100).WithMessage("Tên cấp bậc không được vượt quá 100 ký tự.");

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Thứ tự phải lớn hơn hoặc bằng 0.");

            RuleFor(x => x.BackgroundColor)
                .MaximumLength(20).WithMessage("Màu nền không được vượt quá 20 ký tự.")
                .When(x => !string.IsNullOrWhiteSpace(x.BackgroundColor));

            RuleFor(x => x.Color)
                .MaximumLength(20).WithMessage("Màu chữ không được vượt quá 20 ký tự.")
                .When(x => !string.IsNullOrWhiteSpace(x.Color));
        }
    }
}