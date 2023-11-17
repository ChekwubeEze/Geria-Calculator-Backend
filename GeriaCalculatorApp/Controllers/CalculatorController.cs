using Geria.Core.Auth;
using Geria.Core.Infrastructure.Services.Calculatormanagement;
using Geria.Data.Domain.Model.Calculator.Entities;
using GeriaCalculatorApp.Application.Validations;
using GeriaCalculatorApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GeriaCalculatorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorServices _calculator;
        private readonly IIdentityService _identity;

        public CalculatorController(ICalculatorServices calculator, IIdentityService identity)
        {
            _calculator = calculator;
            _identity = identity;
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = CustomAuthenticationSchemes.TokenScheme)]
        [ProducesResponseType(typeof(CalculationResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CalculatorPost([FromHeader(Name = "X_USER_TOKEN")] string X_USER_TOKEN, CalculatorRequest calculatorRequest)
        {
            try
            {
                var user = _identity.CurrentUser;
                var validator = new CalculatorRequestValidator();
                var result = validator.Validate(calculatorRequest);
                if (result.IsValid)
                {
                    var value = _calculator.Calculation(calculatorRequest.NumberO_One, calculatorRequest.NumberO_Two, calculatorRequest.Sign);
                    var input = new InputData()
                    {
                        FirstNumber = calculatorRequest.NumberO_One,
                        LastNumber = calculatorRequest.NumberO_Two,
                        Sign = calculatorRequest.Sign,
                        Result = value,
                        UserName = user.UserName
                    };
                    var data = _calculator.Create(input);
                    var response = new CalculationResponse() 
                    {
                        Operation = $"{input.FirstNumber} {input.Sign} {input.LastNumber}",
                        Result = input.Result
                    };

                    return Ok(response);
                }
                throw new Exception(validator.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost, Route("all")]
        [ProducesResponseType(typeof(CalculationResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CalculatorPostAll(CalculatorRequest calculatorRequest)
        {
            try
            {
                var validator = new CalculatorRequestValidator();
                var result = validator.Validate(calculatorRequest);
                if (result.IsValid)
                {
                    var value = _calculator.Calculation(calculatorRequest.NumberO_One, calculatorRequest.NumberO_Two, calculatorRequest.Sign);
                    var input = new InputData()
                    {
                        FirstNumber = calculatorRequest.NumberO_One,
                        LastNumber = calculatorRequest.NumberO_Two,
                        Sign = calculatorRequest.Sign,
                        Result = value,
                    };
                    var data = await _calculator.Create(input);
                    var response = new CalculationResponse()
                    {
                        Operation = $"{data.FirstNumber} {data.Sign} {data.LastNumber}",
                        Result = data.Result
                    };

                    return Ok(response);
                }
                throw new Exception(validator.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = CustomAuthenticationSchemes.TokenScheme)]
        [ProducesResponseType(typeof(CalculationResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CalculatorPost([FromHeader(Name = "X_USER_TOKEN")] string X_USER_TOKEN)
        {
            try
            {
                var user = _identity.CurrentUser;
                var data = _calculator.GetAllByUser(user.UserName);
                var response = data.Select(x => new CalculationResponse()
                {
                    Operation = $"{x.FirstNumber} {x.Sign} {x.LastNumber}",
                    Result = x.Result
                });

                return Ok(response);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet, Route("all")]
        [ProducesResponseType(typeof(CalculationResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CalculatorGetAll()
        {
            try
            {
                var data = _calculator.GetAll();
                var response = data.Select(x => new CalculationResponse()
                {
                    Operation = $"{x.FirstNumber} {x.Sign} {x.LastNumber}",
                    Result = x.Result
                });

                return Ok(response);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
