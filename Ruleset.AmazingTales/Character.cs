using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace AmazingTales
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public FileInfo Image { get; set; }
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
