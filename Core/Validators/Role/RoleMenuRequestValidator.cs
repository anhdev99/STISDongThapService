using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.Role
{
    public class RoleMenuRequestValidator : AbstractValidator<RoleMenuRequest>
    {
        public RoleMenuRequestValidator()
        {
            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("Vai trò không hợp lệ.");

            RuleFor(x => x.MenuIds)
                .NotNull().WithMessage("Danh sách menu không được để trống.")
                .Must(x => x.Count > 0).WithMessage("Phải chọn ít nhất một menu.");
        }
    }
}