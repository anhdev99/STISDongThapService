using FluentValidation;
using Core.DTOs.Requests;

namespace Core.Validators.TaskType
{
    public class CreateTaskTypeRequestValidator : AbstractValidator<CreateTaskTypeRequest>
    {
        public CreateTaskTypeRequestValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Mã loại nhiệm vụ không được để trống.")
                .MaximumLength(50).WithMessage("Mã loại nhiệm vụ không được vượt quá 50 ký tự.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên loại nhiệm vụ không được để trống.")
                .MaximumLength(100).WithMessage("Tên loại nhiệm vụ không được vượt quá 100 ký tự.");
        }
    }
}