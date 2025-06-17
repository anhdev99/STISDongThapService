using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.Position
{
    public class CreatePositionRequestValidator : AbstractValidator<CreatePositionRequest>
    {
        public CreatePositionRequestValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Mã chức vụ không được để trống.")
                .MaximumLength(50).WithMessage("Mã chức vụ không được vượt quá 50 ký tự.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên chức vụ không được để trống.")
                .MaximumLength(100).WithMessage("Tên chức vụ không được vượt quá 100 ký tự.");

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Thứ tự phải lớn hơn hoặc bằng 0.");
        }
    }
}