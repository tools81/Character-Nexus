using Utility;

namespace Template
{
    internal class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }

        public string? CharacterSheet { get; set; }       

        public byte[] BuildCharacterSheet()
        {
            throw new NotImplementedException();
        }

        private CharacterSegment GetCharacterSegment() => throw new NotImplementedException();
    }
}
