using Geria.Core.Auth;
using Geria.Core.Auth.Token;
using Geria.Core.Infrastructure.Services.UserManagement;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace GeriaCalculatorApp.Auth.Token
{
    public class SimpleAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
    {
        protected string SignInScheme => Options.SignInScheme;
        private readonly IJwtHandler _jwtHandler;
        private readonly IUserManagmentServices _securityService;

        public SimpleAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IJwtHandler jwtHandler,
            IUserManagmentServices securityService) : base(options, logger, encoder, clock)
        {
            _jwtHandler = jwtHandler;
            _securityService = securityService;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string errorReason;
            if (!Context.Request.Headers.TryGetValue(Options.XUserTokenHeader, out StringValues headerValue))
            {
                errorReason = "Missing or malformed 'X_USER_TOKEN' header";
                return await Task.FromResult(AuthenticateResult.Fail(errorReason));
            }
            var token = headerValue.First();
            try
            {
                var jwtValidationResult = _jwtHandler.Validate(token);
                if (jwtValidationResult.Success)
                {
                    var jwtPayLoad = jwtValidationResult.JwtPayLoad;
                    var user = _securityService.GetUserByEmail(jwtPayLoad.Id);
                    if (user == null)
                    {
                        errorReason = "User not Found";
                        return await Task.FromResult(AuthenticateResult.Fail(errorReason));
                    }
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Authentication, Options.Authentication),
                        new Claim("X_USER_TOKEN", token),
                        new Claim("sub", jwtPayLoad.Id),
                        new Claim(ClaimTypes.Name, jwtPayLoad.Id)
                    };
                    var id = new ClaimsIdentity(claims, CustomAuthenticationSchemes.TokenScheme);
                    var identity = new ClaimsIdentity(id);
                    Options.Identity = identity;
                    var result = Task.FromResult(
                        AuthenticateResult.Success(
                            new AuthenticationTicket(
                                new ClaimsPrincipal(Options.Identity),
                                new AuthenticationProperties(),
                                Scheme.Name)));
                    return await result;
                }
                errorReason = string.Join("; ", jwtValidationResult.Errors);
                return await Task.FromResult(AuthenticateResult.Fail(errorReason));
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return await Task.FromResult(AuthenticateResult.Fail(e));
            }
        }
    }
}
