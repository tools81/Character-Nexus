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
        public string Alias { get; set; }
        public string Image { get; set; }
        public string Notes { get; set; }
        public string Residence { get; set; }
        public string Telex { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public List<Trait> Traits { get; set; }
        public List<Talent> Talents { get; set; }
        public Goal Goal { get; set; }
        public int BrowniePoints { get; set; }
        public List<Gear> Gears { get; set; }
        public List<Weapon> Weapons { get; set; }
        public CharacterSegment CharacterSegment => GetCharacterSegment();

        public string CharacterSheet { get; set; }

        public byte[] BuildCharacterSheet()
        {
            var dict = new Dictionary<string, string>
            {
                { "Name", Name },
                { "Alias", Alias },
                { "Description", Description },
                { "Notes", Notes },
                { "Residence", Residence },
                { "Telex", Telex },
                { "Phone", Phone }
            };

            for (int i = 0; i < Traits.Count; i++)
            {
                dict.Add($"Trait [{i}]", Traits[i].Value.ToString());
            }

            for (int i = 0; i < Talents.Count; i++)
            {
                dict.Add($"Talent [{i}]", Talents[i].Name);
            }

            dict.Add("Goal", Goal.Name);

            for (int i = 0; i < Math.Min(BrowniePoints, 16); i++)
            {
                dict.Add($"Brownie Point [{i}]", "Yes");
            }

            for (int i = 0; i < Gears.Count; i++)
            {
                dict.Add($"Gear [{i}]", Gears[i].Name);
            }

            for (int i = 0; i < Weapons.Count; i++)
            {
                dict.Add($"Weapon [{i}]", Weapons[i].Name);
            }

            return PDFSchema.Generate(dict, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Ghostbusters_Character_Sheet.pdf");
        }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                Image = Image,
                Level = BrowniePoints.ToString(),
                Details = $"Telex: {Telex}"
            };
        }
    }
}
