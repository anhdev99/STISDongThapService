using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.Role
{
    public class ConfigUserRoleRequestValidator : AbstractValidator<ConfigUserRoleRequest>
    {
        public ConfigUserRoleRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Tên người dùng không được để trống.");

            RuleFor(x => x.RoleCode)
                .NotEmpty().WithMessage("Mã vai trò không được để trống.");
        }
    }
}