using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace CharacterNexus.Models
{
    public class CharacterFormRequest
    {
        public IFormFile Image { get; set; }
        public string JsonData { get; set; }
    }
}