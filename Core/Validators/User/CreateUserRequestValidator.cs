using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.User
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Tên đăng nhập không được để trống.")
                .MaximumLength(100).WithMessage("Tên đăng nhập không được vượt quá 100 ký tự.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống.")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Mật khẩu xác nhận không khớp.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Họ không được để trống.")
                .MaximumLength(50).WithMessage("Họ không được vượt quá 50 ký tự.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Tên không được để trống.")
                .MaximumLength(50).WithMessage("Tên không được vượt quá 50 ký tự.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Số điện thoại không được để trống.")
                .Matches(@"^\d{10,15}$").WithMessage("Số điện thoại không hợp lệ.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Email không hợp lệ.");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Phòng ban không hợp lệ.");

            RuleFor(x => x.PositionId)
                .GreaterThan(0).WithMessage("Chức vụ không hợp lệ.");
        }
    }
}