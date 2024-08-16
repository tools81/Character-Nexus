using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace CharacterNexus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;
        private readonly IStorage _storage;

        public ImageController(ILogger<ImageController> logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (HttpContext.Items.TryGetValue("Ruleset", out var rulesetObj) && rulesetObj is IRuleset ruleset)
            {
                _logger.LogInformation($"Character image save request in ruleset {ruleset.Name}");

                var url = await _storage.UploadImageAsync(ruleset.Name, image.FileName, image);

                return Ok(new { Url = url });
            }
            else
            {
                return BadRequest("Invalid ruleset selection.");
            }
        }

        [HttpGet("{imageName}")]
        public IActionResult Get(string imageName)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", imageName);
            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }
            var image = System.IO.File.OpenRead(imagePath);
            return File(image, "image/jpeg"); // Adjust MIME type if necessary
        }
    }
}
