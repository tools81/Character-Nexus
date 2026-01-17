using Utility;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace CharacterNexus
{
    public class RulesetResolutionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEnumerable<IRuleset> _rulesets;

        public RulesetResolutionMiddleware(RequestDelegate next, IEnumerable<IRuleset> rulesets)
        {
            _next = next;
            _rulesets = rulesets;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Read the requested ruleset from query string
            var selectedRuleset = context.Request.Query["ruleset"].ToString();

            // Default to first available ruleset if none is specified
            if (string.IsNullOrWhiteSpace(selectedRuleset))
            {
                selectedRuleset = _rulesets.FirstOrDefault()?.Name;
            }

            // Find the ruleset by name (case-insensitive)
            var ruleset = _rulesets.FirstOrDefault(r =>
                r.Name.Equals(selectedRuleset, System.StringComparison.OrdinalIgnoreCase));

            if (ruleset != null)
            {
                // Store in HttpContext for controllers
                context.Items["Ruleset"] = ruleset;
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
