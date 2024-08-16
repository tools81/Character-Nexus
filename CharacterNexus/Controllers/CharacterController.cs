using Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CharacterNexus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ILogger<CharacterController> _logger;
        private readonly IStorage _storage;

        public CharacterController(ILogger<CharacterController> logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        [HttpGet("new")]
        public IActionResult NewCharacter()
        {
            if (HttpContext.Items.TryGetValue("Ruleset", out var rulesetObj) && rulesetObj is IRuleset ruleset)
            {
                _logger.LogInformation($"New character started for ruleset {ruleset.Name}");

                var character = ruleset.NewCharacter();

                return Ok(ruleset.NewCharacter());
            }
            else
            {
                return BadRequest("Invalid ruleset selection.");
            }
        }        
        
        [HttpGet("load")]
        public IActionResult LoadCharacter(string characterName)
        {
            if (HttpContext.Items.TryGetValue("Ruleset", out var rulesetObj) && rulesetObj is IRuleset ruleset)
            {
                _logger.LogInformation($"Character load requested for {characterName} in ruleset {ruleset.Name}");

                var character = _storage.DownloadCharacterAsync(ruleset.Name, characterName).Result;
                var characterJson = ruleset.LoadCharacter(character);
                return Ok(characterJson);
            }
            else
            {
                return BadRequest("Invalid ruleset selection.");
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveCharacter([FromBody] JObject data)
        {
            if (HttpContext.Items.TryGetValue("Ruleset", out var rulesetObj) && rulesetObj is IRuleset ruleset)
            {
                _logger.LogInformation($"Character save requested in ruleset {ruleset.Name}");

                var result = JsonConvert.SerializeObject(data);
                var character = ruleset.SaveCharacter(result);

                if (character != null)
                {
                    await _storage.UploadCharacterAsync(ruleset.Name, character);
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to create character.");
                }
            }
            else
            {
                return BadRequest("Invalid ruleset selection.");
            }
        }

        [HttpDelete("delete")]
        public IActionResult DeleteCharacter(string characterName)
        {
            if (HttpContext.Items.TryGetValue("Ruleset", out var rulesetObj) && rulesetObj is IRuleset ruleset)
            {
                _logger.LogInformation($"Character delete requested for {characterName} in ruleset {ruleset.Name}");

                _storage.DeleteCharacterAsync(ruleset.Name, characterName);
                return Ok();
            }
            else
            {
                return BadRequest("Invalid ruleset selection.");
            }
        }
    }
}
