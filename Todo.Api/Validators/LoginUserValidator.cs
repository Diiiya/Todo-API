using FluentValidation;
using Todo.Api.DTOs;

namespace Todo.Api.Validators
{
    public class LoginUserValidator : AbstractValidator<LoginUserDTO>
    {
        public LoginUserValidator() 
        {
            RuleFor(user => user.Login)
                .NotNull().WithMessage("Username/Email required");
            
            RuleFor(user => user.Password)
                .NotNull().WithMessage("Password required");
        }
    }
}