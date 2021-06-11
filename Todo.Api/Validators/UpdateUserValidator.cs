using FluentValidation;
using Todo.Api.DTOs;

namespace Todo.Api.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserValidator() 
        {
            RuleFor(user => user.CurrentPassword)
                .NotNull().WithMessage("Current password required");

            RuleFor(user => user.Email)
                .NotNull().WithMessage("Email is required.")
                .Length(6, 254).WithMessage("Email should have between 6 and 254 characters.")
                .EmailAddress().WithMessage("A valid email is required");

            RuleFor(user => user.NewPassword)
                .Length(6, 40).WithMessage("Password should have between 6 and 40 characters.")
                .Matches(@"^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$").WithMessage("The password should contain at least one letter and one number!");
        }  
    }
}