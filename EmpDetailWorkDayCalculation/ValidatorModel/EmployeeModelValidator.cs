using EmpDetailWorkDayCalculation.ViewModels;
using FluentValidation;

namespace EmpDetailWorkDayCalculation.ValidatorModel
{
    public class EmployeeModelValidator : AbstractValidator<EmployeeViewModel>
    {
        public EmployeeModelValidator()
        {
            RuleFor(emp => emp.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(emp => emp.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(emp => emp.JobPosition)
                .NotEmpty().WithMessage("Job position is required.");
        }
    }
}
