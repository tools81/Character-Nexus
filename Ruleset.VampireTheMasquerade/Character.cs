using System;
using System.Collections.Generic;
using Utility;

namespace VampireTheMasquerade
{
    //TODO: Implement thin-blood items
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string Concept { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
        public DateTime DateOfDeath { get; set; } = DateTime.MinValue;
        public string Appearance { get; set; } = string.Empty;
        public string DistinguishingFeatures { get; set; } = string.Empty;
        public string History { get; set; } = string.Empty;
        public Origin Origin { get; set; } = new Origin();
        public Predator Predator { get; set; } = new Predator();
        public string Chronicle { get; set; } = string.Empty;
        public string Ambition { get; set; } = string.Empty;
        public Clan Clan { get; set; } = new Clan();
        public string Sire { get; set; } = string.Empty;
        public string Desire { get; set; } = string.Empty;
        public Generation Generation { get; set; } = new Generation();
        public List<Attribute> Attributes { get; set; }
        public int Health { get; set; }
        public int Willpower { get; set; }
        public List<Skill> Skills { get; set; } = new List<Skill>();
        public List<Specialty> Specialties { get; set; } = new List<Specialty>();
        public List<Discipline> Disciplines { get; set; } = new List<Discipline>();
        public List<Power> Power { get; set; } = new List<Power>();
        public List<Ritual> Rituals { get; set; } = new List<Ritual>();
        public List<Alchemy> Alchemy { get; set; } = new List<Alchemy>();
        public string Resonance { get; set; } = string.Empty;
        public int Hunger { get; set; }
        public int Humanity { get; set; }
        public string ChronicleTenets { get; set; } = string.Empty;
        public string TouchstonesConvictions { get; set; } = string.Empty;
        public int BloodPotency { get; set; }
        public List<Advantage> Advantages { get; set; } = new List<Advantage>();
        public List<Flaw> Flaws { get; set; } = new List<Flaw>();
        public List<Merit> Merits { get; set; } = new List<Merit>();
        public Coterie Coterie { get; set; } = new Coterie();
        public int TotalExperience { get; set; }
        public int SpentExperience { get; set; }

        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }

        public string CharacterSheet { get; set; } = string.Empty;

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
                Level = Humanity,
                LevelName = "Humanity",
                Details = $"{Origin.Name} | {Clan.Name}",
                CharacterSheet = CharacterSheet
            };
        }
    }
}
