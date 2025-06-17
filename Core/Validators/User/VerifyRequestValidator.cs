using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.User
{
    public class VerifyRequestValidator : AbstractValidator<VerifyRequest>
    {
        public VerifyRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Tên đăng nhập không được để trống.");
        }
    }
}