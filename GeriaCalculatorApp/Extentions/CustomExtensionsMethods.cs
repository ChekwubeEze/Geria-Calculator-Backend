using Geria.Core.Auth;
using GeriaCalculatorApp.Auth.Token;

namespace GeriaCalculatorApp.Extentions
{
    public static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                CustomAuthenticationSchemes.TokenScheme;
                options.DefaultChallengeScheme =
                CustomAuthenticationSchemes.TokenScheme;
            }).AddSimpleTokenAuthentication(CustomAuthenticationSchemes.TokenScheme, "This is my lovely token authentication scheme",
            o =>
            {

            });
            return services;
        }
    }
}
