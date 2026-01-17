using Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace CharacterNexus
{
    public class RulesetResolutionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IEnumerable<IRuleset> _rulesets;

        public RulesetResolutionMiddleware(
            RequestDelegate next,
            IConfiguration configuration,
            IEnumerable<IRuleset> rulesets)
        {
            _next = next;
            _configuration = configuration;
            _rulesets = rulesets;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Get ruleset from query string
            var selectedRuleset = context.Request.Query["ruleset"].ToString();

            if (string.IsNullOrWhiteSpace(selectedRuleset))
            {
                await _next(context);
                return;
            }

            // Normalize casing (matches your existing behavior)
            var txtInfo = new CultureInfo("en-US", false).TextInfo;
            selectedRuleset = txtInfo.ToTitleCase(selectedRuleset);

            // Load mapping from config
            var rulesetMapping =
                _configuration
                    .GetSection("MappingsRuleset")
                    .Get<Dictionary<string, string>>();

            if (rulesetMapping == null ||
                !rulesetMapping.TryGetValue(selectedRuleset, out var fullTypeName))
            {
                await _next(context);
                return;
            }

            // Find matching ruleset from DI container
            var ruleset = _rulesets.FirstOrDefault(r =>
                r.GetType().FullName == fullTypeName);

            if (ruleset != null)
            {
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
