using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterNexus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;

        public ImageController(ILogger<ImageController> logger)
        {
            _logger = logger;
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
