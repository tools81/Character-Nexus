using System;
using System.Collections.Generic;
using Utility;

namespace VampireTheMasquerade
{
    //TODO: Implement thin-blood items
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Notes { get; set; }
        public string Concept { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfDeath { get; set; }
        public string Appearance { get; set; }
        public string DistinguishingFeatures { get; set; }
        public string History { get; set; }
        public Predator Predator { get; set; }
        public string Chronicle { get; set; }
        public string Ambition { get; set; }
        public Clan Clan { get; set; }
        public string Sire { get; set; }
        public string Desire { get; set; }
        public int Generation { get; set; }
        public List<Attribute> Attributes { get; set; }
        public int Health { get; set; }
        public int Willpower { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Specialty> Specialties { get; set; }
        public List<Discipline> Disciplines { get; set; }
        public List<Power> Power { get; set; }
        public List<Ritual> Rituals { get; set; }
        public string Resonance { get; set; }
        public int Hunger { get; set; }
        public int Humanity { get; set; }
        public string ChronicleTenets { get; set; }
        public string TouchstonesConvictions { get; set; }
        public int BloodPotency { get; set; }
        public Background Background { get; set; }
        public List<Advantage> Advantages { get; set; }
        public List<Flaw> Flaws { get; set; }        
        public List<Merit> Merits { get; set; }
        public Coterie Coterie { get; set; }
        public int TotalExperience { get; set; }
        public int SpentExperience { get; set; }

        public CharacterSegment CharacterSegment => throw new NotImplementedException();

        public string CharacterSheet => throw new NotImplementedException();

        string ICharacter.CharacterSheet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public byte[] BuildCharacterSheet()
        {
            throw new NotImplementedException();
        }
    }
}
