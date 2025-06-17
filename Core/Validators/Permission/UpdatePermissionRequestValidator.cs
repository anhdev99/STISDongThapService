using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.Permission
{
    public class UpdatePermissionRequestValidator : AbstractValidator<UpdatePermissionRequest>
    {
        public UpdatePermissionRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id quyền không hợp lệ.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên quyền không được để trống.")
                .MaximumLength(100).WithMessage("Tên quyền không được vượt quá 100 ký tự.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Mã quyền không được để trống.")
                .MaximumLength(50).WithMessage("Mã quyền không được vượt quá 50 ký tự.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự.");

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Thứ tự phải lớn hơn hoặc bằng 0.");
        }
    }
}