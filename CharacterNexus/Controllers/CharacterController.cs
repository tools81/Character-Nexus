﻿using Utility.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterNexus.Controllers
{
    public class CharacterController : ControllerBase
    {
        private readonly ILogger<CharacterController> _logger;
        private readonly IStorage _storage;

        public CharacterController(ILogger<CharacterController> logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        [HttpPost("new-character")]
        public IActionResult NewCharacter()
        {
            if (HttpContext.Items.TryGetValue("Ruleset", out var rulesetObj) && rulesetObj is IRuleset ruleset)
            {
                _logger.LogInformation($"New character started for ruleset {ruleset.Name}");
      
                return Ok(ruleset.NewCharacter());
            }
            else
            {
                return BadRequest("Invalid ruleset selection.");
            }
        }

        [HttpPost("save-character")]
        public IActionResult SaveCharacter(ICharacter character)
        {            
            if (HttpContext.Items.TryGetValue("Ruleset", out var rulesetObj) && rulesetObj is IRuleset ruleset)
            {
                _logger.LogInformation($"Character save requested for {character.Name} in ruleset {ruleset.Name}");

                _storage.UploadCharacterAsync(ruleset.Name, character);
                return Ok();
            }
            else
            {
                return BadRequest("Invalid ruleset selection.");
            }
        }
        
        [HttpPost("load-character")]
        public IActionResult LoadCharacter(string characterName)
        {
            if (HttpContext.Items.TryGetValue("Ruleset", out var rulesetObj) && rulesetObj is IRuleset ruleset)
            {
                _logger.LogInformation($"Character load requested for {characterName} in ruleset {ruleset.Name}");

                var character = _storage.DownloadCharacterAsync(ruleset.Name, characterName).Result;
                return Ok(character);
            }
            else
            {
                return BadRequest("Invalid ruleset selection.");
            }
        }

        [HttpPost("delete-character")]
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
