using System;
using System.Collections.Generic;
using System.Reflection;
using Utility;

namespace TMNT
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Animal Animal { get; set; }
        public Alignment Alignment { get; set; }
        public Education Education { get; set; }
        public Mutation Mutation { get; set; }
        public Organization Organization { get; set; }
        public string Origin { get; set; }
        public string Notes { get; set; }
        public string Disposition { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string Size { get; set; }
        public int Level { get; set; }
        public int XP { get; set; }
        public int HitPoints { get; set; }
        public int SDC { get; set; }
        public List<Attribute> Attributes { get; set; }
        public Biped Biped { get; set; }
        public Hand Hand { get; set; }
        public Look Look { get; set; }
        public NaturalWeapon NaturalWeapon { get; set; }
        public Psionic Psionic { get; set; }
        public Speech Speech { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Weapon> Weapons { get; set; }
        public List<Equipment> Equipments { get; set; }
        public Vehicle Vehicle { get; set; }
        public Armor Armor { get; set; }

        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }        
        public string CharacterSheet { get; set; } = string.Empty;

        public byte[] BuildCharacterSheet()
        {
            throw new NotImplementedException();
        }

        private CharacterSegment GetCharacterSegment()
        {
            throw new NotImplementedException();
        }
    }
}