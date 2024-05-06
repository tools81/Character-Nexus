using Azure.Storage.Blobs;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("save-character")]
        public IActionResult CreateCharacter() //ICharacter character
        {
            if (HttpContext.Items.TryGetValue("Ruleset", out var rulesetObj) && rulesetObj is IRuleset ruleset)
            {
                //var character = ruleset.CreateCharacter(character);

                return Ok(); //return Ok(character);
            }
            else
            {
                return BadRequest("Invalid ruleset selection.");
            }
        }           
    }
}
