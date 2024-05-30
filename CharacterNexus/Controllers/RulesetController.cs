using Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace CharacterNexus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RulesetController : ControllerBase
    {
        private readonly ILogger<RulesetController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IStorage _storage;

        public RulesetController(ILogger<RulesetController> logger, IConfiguration configuration, IStorage storage)
        {
            _logger = logger;
            _configuration = configuration;
            _storage = storage;
        }

        [HttpGet("rulesets")]
        public IActionResult GetRulesets()
        {
            _logger.LogInformation($"Requested rulesets");

            var rulesetList = new List<IRuleset>();
            var rulesetMapping = _configuration.GetSection("MappingsRuleset").Get<Dictionary<string, string>>();
            var assemblyPath = _configuration.GetSection("Settings").GetValue("AssemblyPath", "");
            
            foreach(string rulesetName in rulesetMapping.Values)
            {
                try
                {
                    var path = $"{assemblyPath}{rulesetName}.dll";
                    var assembly = Assembly.LoadFrom(path);

                    var rulesetType = assembly.GetType(rulesetName.SwapTextAroundPeriod());

                    if (rulesetType != null)
                    {
                        var instance = Activator.CreateInstance(rulesetType) as IRuleset;
                        rulesetList.Add(instance);
                    }
                }
                catch(Exception ex)
                {
                    return BadRequest(ex);
                }
            }

            return Ok(rulesetList);
        }

        [HttpGet("characters")]
        public IActionResult GetCharacters()
        {
            if (HttpContext.Items.TryGetValue("Ruleset", out var rulesetObj) && rulesetObj is IRuleset ruleset)
            {
                _logger.LogInformation($"Requested characters retrieval for ruleset: {ruleset.Name}");

                var characters = _storage.DownloadCharactersByRulesetAsync(ruleset.Name).Result;
                return Ok(characters);
            }
            else
            {
                return BadRequest("Invalid ruleset selection.");
            }
        }       
    }
}
