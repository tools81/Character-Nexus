using Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using System.Globalization;
using System.Reflection;

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
            Console.WriteLine($"Ruleset to query: {selectedRuleset}");

            TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
            selectedRuleset = txtInfo.ToTitleCase(selectedRuleset);
            Console.WriteLine($"Ruleset post camelCase: {selectedRuleset}");

            // Retrieve the ruleset mapping from configuration
            var rulesetMapping = _configuration.GetSection("MappingsRuleset").Get<Dictionary<string, string>>();
            var assemblyPath = _configuration.GetSection("Settings").GetValue("AssemblyPath", "");

            if (rulesetMapping.TryGetValue(selectedRuleset, out var rulesetName))
            {
                Console.WriteLine($"Ruleset mapping: {rulesetName}");
                // Resolve the ruleset implementation dynamically
                var path = $"{assemblyPath}{rulesetName}.dll";
                Console.WriteLine($"Assembly path: {path}");
                var assembly = Assembly.LoadFrom(path);

                var rulesetType = assembly.GetType(rulesetName.SwapTextAroundPeriod());

                if (rulesetType != null)
                {
                    var ruleset = Activator.CreateInstance(rulesetType) as IRuleset;
                    Console.WriteLine($"Ruleset name: {ruleset.Name}");

                    if (ruleset != null)
                    {
                        // Store the resolved ruleset in the HttpContext.Items for subsequent controllers
                        context.Items["Ruleset"] = ruleset;
                    }
                }              
            }

            await _next(context);
        }       
    }

    public static class RulesetResolutionMiddlewareExtensions
    {
        public static IApplicationBuilder UseRulesetMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RulesetResolutionMiddleware>();
        }
    }
}
