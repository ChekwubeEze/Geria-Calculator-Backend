using FluentValidation;
using GeriaCalculatorApp.ViewModels;

namespace GeriaCalculatorApp.Application.Validations
{
    public class UserRegisterRequestValidator : NullReferenceAbstractValidator<UserRegisterRequest>
    {
        public UserRegisterRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.Password);
        }
    }
}
