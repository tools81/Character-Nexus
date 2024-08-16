using Utility;
using System;

namespace GiJoe
{
    public class Character : ICharacter
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CharacterSegment CharacterSegment => throw new NotImplementedException();
    }
}
