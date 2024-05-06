using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterNexus
{
    public class RulesetResolutionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public RulesetResolutionMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Retrieve the user's selected ruleset (from query parameter, header, etc.)
            var selectedRuleset = context.Request.Query["ruleset"].ToString();

            // Retrieve the ruleset mapping from configuration
            var rulesetMapping = _configuration.GetSection("MappingsRuleset").Get<Dictionary<string, string>>();

            if (rulesetMapping.TryGetValue(selectedRuleset, out var rulesetType))
            {
                // Resolve the ruleset implementation dynamically
                var ruleset = Activator.CreateInstance(Type.GetType(rulesetType)) as IRuleset;

                if (ruleset != null)
                {
                    // Store the resolved ruleset in the HttpContext.Items for subsequent controllers
                    context.Items["Ruleset"] = ruleset;
                }
            }

            await _next(context);
        }
    }
}
