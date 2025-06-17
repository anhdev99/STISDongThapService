using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.Role
{
    public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
    {
        public UpdateRoleRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id không hợp lệ.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên vai trò không được để trống.")
                .MaximumLength(100).WithMessage("Tên vai trò không được vượt quá 100 ký tự.");

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("Mô tả không được vượt quá 250 ký tự.");

            RuleFor(x => x.DisplayName)
                .NotEmpty().WithMessage("Tên hiển thị không được để trống.")
                .MaximumLength(100).WithMessage("Tên hiển thị không được vượt quá 100 ký tự.");

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Thứ tự phải lớn hơn hoặc bằng 0.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Mã vai trò không được để trống.")
                .MaximumLength(50).WithMessage("Mã vai trò không được vượt quá 50 ký tự.");

            RuleFor(x => x.Color)
                .MaximumLength(20).WithMessage("Màu không được vượt quá 20 ký tự.")
                .When(x => !string.IsNullOrWhiteSpace(x.Color));
        }
    }
}