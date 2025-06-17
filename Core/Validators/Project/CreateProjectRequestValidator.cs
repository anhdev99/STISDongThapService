using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.Project
{
    public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
    {
        public CreateProjectRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên dự án không được để trống.")
                .MaximumLength(200).WithMessage("Tên dự án không được vượt quá 200 ký tự.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Nội dung không được để trống.")
                .MaximumLength(2000).WithMessage("Nội dung không được vượt quá 2000 ký tự.");

            RuleFor(x => x.Target)
                .NotEmpty().WithMessage("Mục tiêu không được để trống.")
                .MaximumLength(1000).WithMessage("Mục tiêu không được vượt quá 1000 ký tự.");

            RuleFor(x => x.SectorId)
                .GreaterThan(0).WithMessage("Lĩnh vực không hợp lệ.");
        }
    }
}