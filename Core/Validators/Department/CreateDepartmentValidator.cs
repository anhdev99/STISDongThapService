using Core.DTOs.Requests;
using FluentValidation;

namespace Core.Validators.Departments;

public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên không được để trống")
            .MaximumLength(200).WithMessage("Tên không quá 200 ký tự");

        RuleFor(x => x.Code)
            .NotNull().WithMessage("Mã code không được để trống")
            .MaximumLength(100).WithMessage("Mã code không được vượt quá 100 kí tự");

        RuleFor(x => x.Order)
            .NotNull().WithMessage("Order Không được để trống");
    }
}