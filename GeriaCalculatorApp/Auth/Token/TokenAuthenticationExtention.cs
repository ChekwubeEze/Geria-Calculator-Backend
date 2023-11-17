using Microsoft.AspNetCore.Authentication;

namespace GeriaCalculatorApp.Auth.Token
{
    public static class TokenAuthenticationExtention
    {
        public static AuthenticationBuilder AddSimpleTokenAuthentication(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<TokenAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TokenAuthenticationOptions, SimpleAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
