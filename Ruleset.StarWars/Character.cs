using Utility;

namespace StarWars
{
    internal class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;        
        public Background Background { get; set; } = new Background();
        public List<Obligation> Obligations { get; set; } = new List<Obligation>();
        public Ambition Ambition { get; set; } = new Ambition();
        public Cause Cause { get; set; } = new Cause();
        public Relationship Relationship { get; set; } = new Relationship();
        public Species Species { get; set; } = new Species();
        public string Height { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string Build { get; set; } = string.Empty;
        public string HairColor { get; set; } = string.Empty;
        public string EyeColor { get; set; } = string.Empty;
        public string SkinColor { get; set; } = string.Empty;
        public string IdentifyingMarks { get; set; } = string.Empty;
        public string Personality { get; set; } = string.Empty;
        public int Morality { get; set; } = 50;
        public List<Attribute> Attributes { get; set; } = new List<Attribute>();
        public Career Career { get; set; } = new Career();
        public List<Specialization> Specializations { get; set; } = new List<Specialization>();
        public List<Talent_Tree> Talent_Trees { get; set; } = new List<Talent_Tree>();
        public List<Skill> Skills { get; set; } = new List<Skill>();
        public List<Weapon> Weapons { get; set; } = new List<Weapon>();
        public List<Armor> Armors { get; set; } = new List<Armor>();
        public List<Gear> Gears { get; set; } = new List<Gear>();
        public int ForceRating { get; set; }
        public int ForcePoints { get; set; }
        public int WoundThreshold { get; set; }
        public int StrainThreshold { get; set; }
        public int MeleeDefense { get; set; }
        public int RangedDefense { get; set; }
        public int Soak { get; set; }
        public int EncumbranceThreshold { get; set; }
        public int Credits { get; set; } = 500;
        public int Experience { get; set; }
        public string Notes { get; set; } = string.Empty;

        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }

        public string? CharacterSheet { get; set; }       

        public byte[] BuildCharacterSheet()
        {
            throw new NotImplementedException();
        }

        private CharacterSegment GetCharacterSegment() => throw new NotImplementedException();
    }
}
