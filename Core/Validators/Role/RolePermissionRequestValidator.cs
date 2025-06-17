using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.Role
{
    public class RolePermissionRequestValidator : AbstractValidator<RolePermissionRequest>
    {
        public RolePermissionRequestValidator()
        {
            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("Vai trò không hợp lệ.");

            RuleFor(x => x.PermissionIds)
                .NotNull().WithMessage("Danh sách quyền không được để trống.")
                .Must(x => x.Count > 0).WithMessage("Phải chọn ít nhất một quyền.");
        }
    }
}