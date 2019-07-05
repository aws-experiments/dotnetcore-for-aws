
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcore_for_aws.Infra.Auth
{
    public class AuthorizationHeaderOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var hasAuthorize = context.ApiDescription.ControllerAttributes().OfType<AuthorizeAttribute>().Any() ||
                context.ApiDescription.ActionAttributes().OfType<AuthorizeAttribute>().Any();
            if(hasAuthorize)
            {
                operation.Responses.TryAdd("401", new Response { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
            {
                new Dictionary<string, IEnumerable<string>>
                {
                    { "oauth2", new [] { "identityapi" } }
                }
            };
            }
        }

    }
}
