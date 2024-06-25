using Utility;

namespace Marvel
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Rank { get; set; }

        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                ImageUrl = ImageUrl,
                Level = Rank,
                Details = $"{Origin} | {Occupation}"
            };
        }
    }
}