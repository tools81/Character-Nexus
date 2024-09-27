using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Utility;

namespace BladeRunner
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Appearance { get; set; }
        public string Image { get; set; }
        public int Health { get; set; }
        public int Resolve { get; set; }       
        public int Chinyen { get; set; }
        public int PromotionPoints { get; set; }
        public int HumanityPoints { get; set; }
        public string Notes { get; set; }
        public Origin? Origin { get; set; }
        public Archetype? Archetype { get; set; }
        public List<Attribute>? Attributes { get; set; }
        public Tenure? Tenure { get; set; }
        public List<Skill>? Skills { get; set; }
        public List<Specialty>? Specialties { get; set; }
        public Memory? Memory { get; set; }
        public Relationship? Relationship { get; set; }
        public string FavoredGear { get; set; }
        public string SignatureItem { get; set; }
        public string Home { get; set; }
        public List<Gear>? Gears { get; set; }
        public List<Armor>? Armors { get; set; }
        public List<Weapon>? Weapons { get; set; }
        public List<Vehicle>? Vehicles { get; set; }

        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }
        public string? CharacterSheet { get; set; }

        public byte[] BuildCharacterSheet()
        {
            var dict = new Dictionary<string, string>();

            dict.Add("Name", Name);
            dict.Add("Notes", Notes);

            return PDFSchema.Generate(dict, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Character_Sheet.pdf");
        }


        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                Image = Image,
                Details = $"{Origin.Name} | {Archetype.Name} | {Tenure.Name}"
            };
        }
    }
}
