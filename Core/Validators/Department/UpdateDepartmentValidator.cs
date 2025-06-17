using Core.DTOs.Requests;
using FluentValidation;

namespace Core.Validators.Departments;

public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentRequest>
{
    public UpdateDepartmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID không được để trống");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên không được để trống")
            .MaximumLength(200).WithMessage("Tên không quá 200 ký tự");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Mã code không được để trống")
            .MaximumLength(100).WithMessage("Mã code không được vượt quá 100 ký tự");

        RuleFor(x => x.Order)
            .NotNull().WithMessage("Order không được để trống");
    }
}