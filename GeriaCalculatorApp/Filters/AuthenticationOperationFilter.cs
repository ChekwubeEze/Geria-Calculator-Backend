using Geria.Core.Auth;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GeriaCalculatorApp.Filters
{
    public class AuthenticationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filterpipline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterpipline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);

            var allowAnonymous = filterpipline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);
            if (isAuthorized && !allowAnonymous)
            {
                var filterMetadatas = filterpipline.Select(filterInfo => filterInfo.Filter).First(x => x is AuthorizeFilter) as AuthorizeFilter;

                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();
                if (filterMetadatas?.Policy != null)
                {
                    var authenticationScheme = filterMetadatas.Policy.AuthenticationSchemes.ToList();
                    var useTokenScheme = authenticationScheme.Contains(CustomAuthenticationSchemes.TokenScheme);
                    var useSimpleTokenScheme = authenticationScheme.Contains(CustomAuthenticationSchemes.SimpleTokenScheme);

                    if (useTokenScheme)
                    {
                        operation.Parameters.Add(new OpenApiParameter
                        {
                            Name = "X_USER_TOKEN",
                            In = ParameterLocation.Header,
                            Description = "Token from User signin request for the user originating the request",
                            Required = true,
                            Schema = new OpenApiSchema
                            {
                                Type = "string"
                            }

                        });
                    }

                    if (useSimpleTokenScheme)
                    {
                        operation.Parameters.Add(new OpenApiParameter
                        {
                            Name = "X_USER_TOKEN",
                            In = ParameterLocation.Header,
                            Description = "Token from User signin request for the user originating the request",
                            Required = true,
                            Schema = new OpenApiSchema
                            {
                                Type = "string"
                            }
                        });
                    }
                }
            }
        }
    }
}
