using System;

namespace Utility
{
    public interface ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }       
        public CharacterSegment CharacterSegment { get; }
    }
}
