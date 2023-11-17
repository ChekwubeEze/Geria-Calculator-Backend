using Geria.Core.Application.Configurations;
using Geria.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Core.Auth.Token
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtOptions _options;
        private readonly JwtHeader _jwtHeader;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        public JwtHandler(IOptions<JwtOptions> options)
        {
            _options = options.Value;
            SecurityKey security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var signingCredentials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);
            _jwtHeader = new JwtHeader(signingCredentials);
        }
        JsonWebToken IJwtHandler.Create(string username)
        {
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddMinutes(_options.ExpiryMinutes);
            var centuryBegin = new DateTime(1970, 1, 1).ToUniversalTime();
            var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            var iat = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);
            var payLoad = new JwtPayload
            {
                {"sub", username},
                {"iss", _options.Issuer },
                {"iat", iat },
                {"exp", exp},
                {"unique_name", username },
                {"aud", _options.Issuer }
            };
            var jwt = new JwtSecurityToken(_jwtHeader, payLoad);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);
            return new JsonWebToken
            {
                AccessToken = token,
                Expires = exp
            };
        }

        JwtValidationResult IJwtHandler.Validate(string Token)
        {
            var jwtValidationResult = new JwtValidationResult();
            try
            {
                if (_jwtSecurityTokenHandler.CanReadToken(Token))
                {
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = _options.ValidateIssuerSigningKey,
                        ValidIssuer = _options.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                        ClockSkew = TimeSpan.Zero,
                        ValidateAudience = _options.ValidateAudience,
                        ValidateLifetime = _options.ValidateLifetime,
                    };
                    var claims = _jwtSecurityTokenHandler.ValidateToken(Token, validationParameters, out var validatedToken);
                    jwtValidationResult.JwtPayLoad = new JwtPayLoad()
                    {
                        ValidTo = validatedToken.ValidTo,
                        Id = claims.Identity.Name
                    };
                }
                else
                {
                    jwtValidationResult.AddErrors("Token Verification Failed");
                }
            }
            catch (ArgumentException e)
            {
                jwtValidationResult.AddErrors("Unable to decod token");
                throw e;
            }
            catch (Exception e)
            {
                var error = CreateErrorDescription(e);
                jwtValidationResult.AddErrors(error);
            }
            return jwtValidationResult;
        }

        private static string CreateErrorDescription(Exception authFailure)
        {
            IEnumerable<Exception> exceptions;
            if (authFailure is AggregateException agEx)
            {
                exceptions = agEx.InnerExceptions;
            }
            else
            {
                exceptions = new[] { authFailure };
            }

            var messages = new List<string>();

            foreach (var ex in exceptions)
            {
                // Order sensitive, some of these exceptions derive from others
                // and we want to display the most specific message possible.
                if (ex is SecurityTokenInvalidAudienceException)
                {

                    messages.Add("The audience is invalid");
                }
                else if (ex is SecurityTokenInvalidIssuerException)
                {
                    messages.Add("The issuer is invalid");
                }
                else if (ex is SecurityTokenNoExpirationException)
                {
                    messages.Add("The token has no expiration");
                }
                else if (ex is SecurityTokenInvalidLifetimeException)
                {
                    messages.Add("The token lifetime is invalid");
                }
                else if (ex is SecurityTokenNotYetValidException)
                {
                    messages.Add("The token is not valid yet");
                }
                else if (ex is SecurityTokenExpiredException)
                {
                    throw new Exception("The token has expired" + "401");

                }
                else if (ex is SecurityTokenSignatureKeyNotFoundException)
                {
                    messages.Add("The signature key was not found");
                }
                else if (ex is SecurityTokenInvalidSignatureException)
                {
                    messages.Add("The signature is invalid");
                }
            }

            return string.Join("; ", messages);
        }
    }
}
