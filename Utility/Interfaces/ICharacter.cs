using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Utility
{
    public interface ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }       
        public CharacterSegment CharacterSegment { get; }
    }
}
