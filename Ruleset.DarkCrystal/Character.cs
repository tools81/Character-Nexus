using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Utility;

namespace DarkCrystal
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string Image { get; set; } = string.Empty;
        public List<Skill> Skills { get; set; } = new List<Skill>();
        public Clan Clan { get; set; } = new Clan();
        public Gender Gender { get; set; } = new Gender();
        public List<Flaw> Flaws { get; set; } = new List<Flaw>();
        public List<Trait> Traits { get; set; } = new List<Trait>();
        public List<Specialization> Specializations { get; set; } = new List<Specialization>();
        public List<Gear> Gear { get; set; } = new List<Gear>();
        public string Notes { get; set; } = string.Empty;
        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }
        public string CharacterSheet { get; set; } = string.Empty;

        public byte[] BuildCharacterSheet()
        {
            var dict = new Dictionary<string, string>
            {
                { "Name", Name },
                { "Clan", Clan.Name },
                { "Gender", Gender.Name },
                { "Notes", Notes}
            };

            foreach(var skill in Skills)
            {
                if (skill.Trained)
                {
                    dict.Add(skill.Name, "true");
                }
            }

            foreach(var specialization in Specializations)
            {
                dict.Add($"Specialization {specialization.Name}", "true");

                if (specialization.Master)
                {
                    dict.Add($"Mastery {specialization.Name}", "true");
                }
            }

            for (int i = 0; i < Traits.Count; i++)
            {
                if (i < 6) //Maximum lines on character sheet
                {
                    dict.Add($"Trait {i + 1}", Traits[i].Name);
                    dict.Add($"Trait {i + 1} Description", Traits[i].Description);
                }
            }

            for (int i = 0; i < Gear.Count; i++)
            {
                if (i < 6) //Maximum lines on character sheet
                {
                    dict.Add($"Gear {i + 1}", Gear[i].Name);
                    dict.Add($"Gear {i + 1} Description", Gear[i].Description);
                }
            }

            for (int i = 0; i < Flaws.Count; i++)
            {
                if (i < 6) //Maximum lines on character sheet
                {
                    dict.Add($"Flaw {i + 1}", Flaws[i].Name);
                    dict.Add($"Flaw {i + 1} Description", Flaws[i].Description);
                }
            }            

            return PDFSchema.Generate(dict, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Dark_Crystal_Character_Sheet.pdf");
        }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                Image = Image,
                Details = $"{Clan?.Name}",
                CharacterSheet = CharacterSheet
            };
        }
    }
}