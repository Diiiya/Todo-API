using FluentValidation;
using Todo.Api.DTOs;
using Todo.Api.Models;

namespace Todo.Api.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDTO>
    {
        public CreateUserValidator() 
        {
            RuleFor(user => user.Username)
                .NotNull().WithMessage("Username required")
                .Length(3, 30).WithMessage("Username should have between 3 and 30 characters.")
                .Matches(@"^(?:.*[A-Za-z0-9])$").WithMessage("You can only use letters and numbers!");

            RuleFor(user => user.Email)
                .NotNull().WithMessage("Email required")
                .Length(6, 254).WithMessage("Email should have between 6 and 254 characters.")
                .EmailAddress().WithMessage("A valid email is required");

            RuleFor(user => user.Password)
                .NotNull().WithMessage("Password required")
                .Length(6, 40).WithMessage("Password should have between 6 and 40 characters.")
                .Matches(@"^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$").WithMessage("The password should contain at least one letter and one number!");
        }        
    }
}