using Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CharacterNexus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RulesetController : ControllerBase
    {
        private readonly ILogger<RulesetController> _logger;
        private readonly IEnumerable<IRuleset> _rulesets;
        private readonly IStorage _storage;

        public RulesetController(
            ILogger<RulesetController> logger,
            IEnumerable<IRuleset> rulesets,
            IStorage storage)
        {
            _logger = logger;
            _rulesets = rulesets;
            _storage = storage;
        }

        [HttpGet("rulesets")]
        public IActionResult GetRulesets()
        {
            _logger.LogInformation("Requested rulesets");

            return Ok(_rulesets);
        }

        [HttpGet("characters")]
        public IActionResult GetCharacters()
        {
            if (HttpContext.Items.TryGetValue("Ruleset", out var rulesetObj) &&
                rulesetObj is IRuleset ruleset)
            {
                _logger.LogInformation(
                    "Requested characters retrieval for ruleset: {Ruleset}",
                    ruleset.Name);

                var characters =
                    _storage
                        .DownloadCharactersByRulesetAsync(ruleset.Name)
                        .Result;

                return Ok(characters);
            }

            return BadRequest("Invalid ruleset selection.");
        }
    }
}
