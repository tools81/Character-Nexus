using Utility;

namespace Template
{
    internal class Character : ICharacter
    {
        public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Image { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CharacterSegment CharacterSegment => throw new NotImplementedException();

        public string? CharacterSheet { get; set; }

        public CharacterSegment GetCharacterSegment() => throw new NotImplementedException();

        public byte[] BuildCharacterSheet()
        {
            throw new NotImplementedException();
        }
    }
}
