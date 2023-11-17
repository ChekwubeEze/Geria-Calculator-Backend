using FluentValidation;
using GeriaCalculatorApp.ViewModels;

namespace GeriaCalculatorApp.Application.Validations
{
    public class UserLoginRequestValidator : NullReferenceAbstractValidator<UserLoginRequest>
    {
        public UserLoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
