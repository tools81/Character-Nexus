using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Utility;

namespace Ghostbusters
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Personality { get; set; }
        public string Notes { get; set; }
        public string Residence { get; set; }
        public string Telex { get; set; }
        public string Phone { get; set; }
        public List<Trait> Traits { get; set; }
        public List<Talent> Talents { get; set; }
        public Goal Goal { get; set; }
        public int BrowniePoints { get; set; }
        public List<Equipment> Equipments { get; set; }
        public CharacterSegment CharacterSegment => GetCharacterSegment();

        public byte[] CharacterSheet => GetCharacterSheet();

        public byte[] GetCharacterSheet()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("Name", Name);
            dict.Add("Personality", Personality);
            dict.Add("Notes", Notes);
            dict.Add("Residence", Residence);
            dict.Add("Telex", Telex);
            dict.Add("Phone", Phone);

            for (int i = 0; i < Traits.Count; i++)
            {
                dict.Add($"Trait [{i}]", Traits[i].Name);
            }

            for (int i = 0; i < Talents.Count; i++)
            {
                dict.Add($"Talent [{i}]", Talents[i].Name);
            }

            dict.Add("Goal", Goal.Description);
            dict.Add("BrowniePoints", BrowniePoints.ToString());

            for (int i = 0; i < Equipments.Count; i++)
            {
                dict.Add($"Equipment [{i}]", Equipments[i].Name);
            }

            return PDFSchema.Generate(dict, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Character_Sheet.pdf");
        }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                Image = Image,
                Level = BrowniePoints,
                Details = $"{Residence} | {Telex} | {Phone}"
            };
        }
    }
}
