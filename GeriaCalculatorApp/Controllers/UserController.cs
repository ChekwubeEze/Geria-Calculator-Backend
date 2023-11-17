using Geria.Core.Infrastructure.Services.UserManagement;
using Geria.Core.Models;
using GeriaCalculatorApp.Application.Validations;
using GeriaCalculatorApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace GeriaCalculatorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManagmentServices _userManagment;

        public UserController(IUserManagmentServices userManagment)
        {
            _userManagment = userManagment;
        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(UserRegisterResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register(UserRegisterRequest userRegister)
        {
            var logResponse = string.Empty;
            try
            {
                var validator = new UserRegisterRequestValidator();
                var result = await validator.ValidateAsync(userRegister);
                if (result.IsValid)
                {
                    var register = new UserRegistrationRequest()
                    {
                        Email = userRegister.Email,
                        Password = userRegister.Password,
                        UserName = userRegister.UserName,
                    };
                    var registered = await _userManagment.SignUp(register);
                    if (registered == null)
                    {
                        return BadRequest("REGISTRATION WAS NOT SUCCESSFUL");
                    }
                    //await _permissionService.InserPermissionForUser(registered, userRegister.permissions);
                    var response = new UserRegisterResponse()
                    {
                        Id = registered.Id,
                        Email = registered.Email,
                        UserName = registered.UserName,
                    };
                    return Ok(response);
                }
                throw new Exception(result.ToString());
            }
            catch (Exception e)
            {;
                throw e;
            }
        }

        [HttpPost("SignIn")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login(UserLoginRequest userLogin)
        {
            string logResponse = string.Empty;
            try
            {
                var validator = new UserLoginRequestValidator();
                var result = await validator.ValidateAsync(userLogin);
                if (result.IsValid)
                {
                    var userloginRequest = new UserLogin()
                    {
                        Email = userLogin.Email,
                        Password = userLogin.Password,
                    };
                    var response = await _userManagment.Login(userloginRequest);
                    switch (response.LoginResult)
                    {
                        case UserLoginResults.SUCCESSFUL:
                            var logRespose = new LoginResponse
                            {
                                Token = response.JsonWebToken,
                                Username = response.UserName,
                                Id= response.Id,
                            };
                            return Ok(logRespose);
                        case UserLoginResults.NOT_REGISTERED:
                            return BadRequest("NOT_REGISTERED");
                        case UserLoginResults.USER_DOES_NOT_EXIST:
                            return BadRequest("USER_DOES_NOT_EXIST");
                        case UserLoginResults.WRONG_PASSWORD:
                            return BadRequest("WRONG_PASSWORD");
                        case UserLoginResults.NOT_VERIFIED:
                            return BadRequest("NOT_VERIFIED");
                        case UserLoginResults.USER_NOT_ACTIVE:
                            return BadRequest("USER_NOT_ACTIVE");
                        case UserLoginResults.LOCKED_OUT:
                            return BadRequest("USER IS ALREADY LOGED OUT");

                    }
                }
                return BadRequest($"{result.Errors.ToList()}");
                throw new Exception(result.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
