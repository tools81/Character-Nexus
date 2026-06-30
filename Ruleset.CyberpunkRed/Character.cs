using System;
using System.Collections.Generic;
using Utility;

namespace CyberpunkRed
{
    internal class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Role Role { get; set; }
        public Ability Ability { get; set; }
        public Culture Culture { get; set; }
        public List<string> Languages { get; set; } = new List<string>();
        public Personality Personality { get; set; }        
        public Affectation Affectation { get; set; }
        public Motivation Motivation { get; set; }
        public Person Person { get; set; }
        public Possession Possession { get; set; }
        public Background Background { get; set; }
        public Environment Environment { get; set; }
        public Crisis Crisis { get; set; }
        public Relationship Relationship { get; set; }
        public Enemy Enemy { get; set; }
        public Skill Clothing { get; set; }
        public Hairstyle Hairstyle { get; set; }
        public List<Cyberware> Cyberwares { get; set; }
        public List<Drug> Drugs { get; set; }
        public List<Ammunition> Ammunitions { get; set; }   
        public List<Armor> Armors { get; set; }
        public List<Gear> Gears { get; set; }
        public List<Weapon> Weapons { get; set; }
        public List<Skill> Skills { get; set; }

        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }

        public string? CharacterSheet { get; set; }       

        public byte[] BuildCharacterSheet()
        {
            throw new NotImplementedException();
        }

        private CharacterSegment GetCharacterSegment() => throw new NotImplementedException();
    }
}
