using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterNexus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageGenerationController : Controller
    {
        private readonly ILogger<ImageGenerationController> _logger;

        public ImageGenerationController(ILogger<ImageGenerationController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public string Get(string prompt)
        {
            _logger.LogInformation($"Handling image generation on prompt {prompt}");
            return Utility.GenerateImageWithAI.GenerateAndReturnUrl(prompt).Result;
        }
    }
}
