using Utility;

namespace HunterTheReckoning
{
    internal class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Concept { get; set; }
        public string Ambition { get; set; }
        public string Desire { get; set; }
        public string Notes { get; set; }
        public List<string> Touchstones { get; set; }
        public Creed Creed { get; set; }
        public Drive Drive { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Specialty> Specialties { get; set; } = new List<Specialty>();
        public List<Advantage> Advantages { get; set; } = new List<Advantage>();
        public List<Background> Backgrounds { get; set; } = new List<Background>();
        public List<Flaw> Flaws { get; set; } = new List<Flaw>();
        public List<Merit> Merits { get; set; } = new List<Merit>();
        public List<Perk> Perks { get; set; } = new List<Perk>();
        public List<Weapon> Weapons { get; set; }
        public List<Armor> Armors { get; set; }
        public List<Gear> Gears { get; set; }
        public int Health { get; set; }
        public int Willpower { get; set; }
        public int Experience { get; set; }

        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }

        public string? CharacterSheet { get; set; }       

        public byte[] BuildCharacterSheet()
        {
            throw new NotImplementedException();
        }

        private CharacterSegment GetCharacterSegment() => throw new NotImplementedException();
    }
}
