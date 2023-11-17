using FluentValidation;
using GeriaCalculatorApp.ViewModels;

namespace GeriaCalculatorApp.Application.Validations
{
    public class CalculatorRequestValidator : NullReferenceAbstractValidator<CalculatorRequest>
    {
        public CalculatorRequestValidator()
        {
            RuleFor(x => x.NumberO_One).NotEmpty();
            RuleFor(x => x.NumberO_Two).NotEmpty();
            RuleFor(x => x.Sign).Must(x => x == "+" || x == "-" || x == "*" || x == "/");
        }
    }
}
