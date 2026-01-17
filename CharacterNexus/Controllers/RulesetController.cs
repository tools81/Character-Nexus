using Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CharacterNexus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RulesetController : ControllerBase
    {
        private readonly ILogger<RulesetController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IStorage _storage;
        private readonly IEnumerable<IRuleset> _rulesets;

        public RulesetController(
            ILogger<RulesetController> logger,
            IConfiguration configuration,
            IStorage storage,
            IEnumerable<IRuleset> rulesets)
        {
            _logger = logger;
            _configuration = configuration;
            _storage = storage;
            _rulesets = rulesets;
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
