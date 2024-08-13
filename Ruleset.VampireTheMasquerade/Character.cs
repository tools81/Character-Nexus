using Utility;
using System;
using Microsoft.AspNetCore.Http;

namespace VampireTheMasquerade
{
    public class Character : ICharacter
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
        public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CharacterSegment CharacterSegment => throw new NotImplementedException();
    }
}
