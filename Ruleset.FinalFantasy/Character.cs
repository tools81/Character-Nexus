using Utility;

namespace FinalFantasy
{
    internal class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int Level { get; set; }
        public Role Role { get; set; }
        public Job Job { get; set; }
        public Race Race { get; set; }
        public Subrace Subrace { get; set; }
        public List<Attribute> Attributes { get; set; }
        public int Defense { get; set; }
        public int MagicDefense { get; set; }
        public int Vigilance { get; set; }
        public int Speed { get; set; }
        public int MP { get; set; }
        public int HP { get; set; }
        public string Size { get; set; }
        public string Profile { get; set; }
        public List<Item> Items { get; set; }
        public Augmentation Augmentation { get; set; }
        public List<Trait> Traits { get; set; }
        public List<Ability> Abilities { get; set; }
        public LimitBreak LimitBreak { get; set; }

        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }

        public string? CharacterSheet { get; set; }       

        public byte[] BuildCharacterSheet()
        {
            throw new NotImplementedException();
        }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                Image = Image,
                Level = Level.ToString(),
                LevelName = "Level",
                Details = $"{Race.Name} | {Role.Name} | {Job.Name}",
                CharacterSheet = CharacterSheet
            };
        }
    }
}
