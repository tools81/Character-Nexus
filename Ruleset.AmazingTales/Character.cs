using Utility;
using System;
using System.Collections.Generic;

namespace AmazingTales
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<Attribute> Attributes { get; set; }

        public CharacterSegment CharacterSegment {get => GetCharacterSegment();}
        public string CharacterSheet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public byte[] GetCharacterSheet()
        {
            throw new NotImplementedException();
        }


        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                Image = Image
            };
        }
    }
}
