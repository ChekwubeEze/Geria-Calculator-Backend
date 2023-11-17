using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace GeriaCalculatorApp.Auth.Token
{
    public class TokenAuthenticationOptions : AuthenticationSchemeOptions
    {
        public ClaimsIdentity Identity { get; set; }
        public string XUserTokenHeader
        {
            get { return "X_USER_TOKEN"; }
        }
        public string Authentication { get { return "Token"; } }
        public string SignInScheme => "Token";
    }
}
