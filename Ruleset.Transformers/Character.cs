using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Transformers
{
    internal class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public int Level { get; set; }
        public Faction Faction { get; set; }
        public Role Role { get; set; }
        public Origin Origin { get; set; }
        public List<Influence> Influences { get; set; }
        public Focus Focus { get; set; }
        public List<Essence> Essences { get; set; }
        public List<HangUp> HangUps { get; set; }
        public List<Perk> Perks { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Specialization> Specializations { get; set; }

        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }   

        public string CharacterSheet { get; set; }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment() {
                Id = Id,
                Name = Name,
                Image = Image,
                Level = Level.ToString(),
                LevelName = "Level",
                Details = $"{Origin.Name} | {Role.Name}",
                CharacterSheet = CharacterSheet
            };
        }

        public byte[] BuildCharacterSheet()
        {
            var dict = new Dictionary<string, string>
            {
                { "Name", Name },
                { "Description", Description },
                { "Notes", Notes },
                { "Level", Level.ToString() },
                { "Origin", Origin.Name },
                { "Role", Role.Name },
                { "Features", Focus.Name },
                { "Influences", string.Join(", ", Influences.Select(i => i.Name)) },
                { "Hang Ups", string.Join(", ", HangUps.Select(h => h.Name)) },
                { "PERKS", string.Join(", ", Perks.Select(p => p.Name)) },
            };

            // Essences — fill score fields and dot tracks
            var essencePrefixMap = new Dictionary<string, (string Abbrev, string Full)>
            {
                { "Strength", ("Str", "Strength") },
                { "Speed",    ("Spd", "Speed") },
                { "Smarts",   ("Sma", "Smarts") },
                { "Social",   ("Soc", "Social") },
            };
            foreach (var essence in Essences)
            {
                if (essencePrefixMap.TryGetValue(essence.Name, out var map))
                {
                    dict[$"{map.Abbrev} Ess"] = essence.Value.ToString();
                    dict[map.Full] = essence.Value.ToString();
                }
            }

            // Skills — fill dots (xxx1–xxx6) up to skill rank
            var skillPrefixMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Athletics",      "ath" },
                { "Brawn",          "bra" },
                { "Conditioning",   "Cond" },
                { "Intimidation",   "int" },
                { "Might",          "mig" },
                { "Acrobatics",     "acr" },
                { "Driving",        "dri" },
                { "Finesse",        "fin" },
                { "Infiltration",   "inf" },
                { "Targeting",      "tar" },
                { "Alertness",      "ale" },
                { "Culture",        "cul" },
                { "Initiative",     "ini" },
                { "Science",        "sci" },
                { "Survival",       "sur" },
                { "Technology",     "tec" },
                { "Animal Handling","ani" },
                { "Deception",      "dec" },
                { "Performance",    "per" },
                { "Persuasion",     "prs" },
                { "Streetwise",     "str" },
            };
            foreach (var skill in Skills)
            {
                if (skillPrefixMap.TryGetValue(skill.Name, out var prefix))
                {
                    for (int i = 1; i <= skill.Value && i <= 6; i++)
                        dict[$"{prefix}{i}"] = "Yes";
                }
            }

            // Specializations — text label (Xxx Sp N) + filled dot (xxx spec N)
            var specPrefixMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Athletics",      "Ath" },
                { "Brawn",          "Bra" },
                { "Intimidation",   "Int" },
                { "Might",          "Mig" },
                { "Acrobatics",     "Acr" },
                { "Driving",        "Dri" },
                { "Finesse",        "Fin" },
                { "Infiltration",   "Inf" },
                { "Targeting",      "Tar" },
                { "Alertness",      "Ale" },
                { "Culture",        "Cul" },
                { "Science",        "Sci" },
                { "Survival",       "Sur" },
                { "Technology",     "Tec" },
                { "Animal Handling","Ani" },
                { "Deception",      "Dec" },
                { "Performance",    "Per" },
                { "Persuasion",     "Prs" },
                { "Streetwise",     "Str" },
            };
            foreach (var group in Specializations.GroupBy(s => s.Type))
            {
                if (specPrefixMap.TryGetValue(group.Key, out var prefix))
                {
                    var specs = group.ToList();
                    for (int i = 0; i < specs.Count && i < 3; i++)
                    {
                        dict[$"{prefix} Sp {i + 1}"] = specs[i].Name;
                        dict[$"{prefix.ToLower()} spec {i + 1}"] = "Yes";
                    }
                }
            }

            return PDFSchema.Generate(dict, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Transformers_Character_Sheet.pdf");
        }
    }
}
