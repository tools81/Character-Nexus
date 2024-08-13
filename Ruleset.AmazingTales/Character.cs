using Utility;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace AmazingTales
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
        public List<Attribute> Attributes { get; set; }

        public CharacterSegment CharacterSegment {get => GetCharacterSegment();}

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                ImageUrl = ImageUrl
            };
        }
    }
}
