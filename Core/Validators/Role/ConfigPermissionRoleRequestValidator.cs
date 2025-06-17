using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.Role
{
    public class ConfigPermissionRoleRequestValidator : AbstractValidator<ConfigPermissionRoleRequest>
    {
        public ConfigPermissionRoleRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id không hợp lệ.");

            RuleFor(x => x.PermissionNames)
                .NotNull().WithMessage("Danh sách quyền không được để trống.")
                .Must(x => x.Count > 0).WithMessage("Phải chọn ít nhất một quyền.");
        }
    }
}