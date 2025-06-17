using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.Sector
{
    public class UpdateSectorRequestValidator : AbstractValidator<UpdateSectorRequest>
    {
        public UpdateSectorRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id không hợp lệ.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Mã lĩnh vực không được để trống.")
                .MaximumLength(50).WithMessage("Mã lĩnh vực không được vượt quá 50 ký tự.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên lĩnh vực không được để trống.")
                .MaximumLength(100).WithMessage("Tên lĩnh vực không được vượt quá 100 ký tự.");

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Thứ tự phải lớn hơn hoặc bằng 0.");
        }
    }
}