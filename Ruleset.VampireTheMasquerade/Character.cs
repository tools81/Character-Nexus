using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Utility;

namespace VampireTheMasquerade
{
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
        public List<Power> Powers { get; set; } = new List<Power>();
        public List<Ritual> Rituals { get; set; } = new List<Ritual>();
        public List<Alchemy> Alchemy { get; set; } = new List<Alchemy>();
        public int Hunger { get; set; }
        public int Humanity { get; set; }
        public string ChronicleTenets { get; set; } = string.Empty;
        public string TouchstonesConvictions { get; set; } = string.Empty;
        public int BloodPotency { get; set; }
        public List<Advantage> Advantages { get; set; } = new List<Advantage>();
        public List<Background> Backgrounds { get; set; } = new List<Background>();
        public List<Flaw> Flaws { get; set; } = new List<Flaw>();
        public List<Merit> Merits { get; set; } = new List<Merit>();
        public Coterie Coterie { get; set; } = new Coterie();
        public int TotalExperience { get; set; }
        public int SpentExperience { get; set; }
        public List<Weapon> Weapons { get; set; }
        public List<Armor> Armors { get; set; }
        public List<Gear> Gears { get; set; }

        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }

        public string CharacterSheet { get; set; } = string.Empty;

        public byte[] BuildCharacterSheet()
        {
            var dict = new Dictionary<string, string>
            {
                { "character_name", Name },
                { "character_ambition", Ambition },
                { "character_concept", Concept },
                { "character_desire", Desire },
                { "character_clan", Clan.Name },
                { "character_generation", Generation.Name },
                { "character_chronicle", Chronicle },
                { "character_predator", Predator.Name },
                { "character_sire", Sire },
                { "character_distinguishing_features", DistinguishingFeatures },
                { "bio_line_1", Age.ToString() },
                { "bio_line_3", DateOfBirth.ToShortDateString() },
                { "bio_line_4", DateOfDeath.ToShortDateString() },
                { "experience_total", TotalExperience.ToString() },
                { "experience_spent", SpentExperience.ToString() }
            };

            var attribute = Attributes.FirstOrDefault(a => a.Name == "Strength");
            if (attribute is not null)
            {
                for (int i = 1; i <= attribute.Value; i++)
                {
                    dict.Add($"strength_{i}", "true");
                }
            }

            attribute = Attributes.FirstOrDefault(a => a.Name == "Dexterity");
            if (attribute is not null)
            {
                for (int i = 1; i <= attribute.Value; i++)
                {
                    dict.Add($"dexterity_{i}", "true");
                }
            }

            attribute = Attributes.FirstOrDefault(a => a.Name == "Stamina");
            if (attribute is not null)
            {
                for (int i = 1; i <= attribute.Value; i++)
                {
                    dict.Add($"stamina_{i}", "true");
                }
            }

            attribute = Attributes.FirstOrDefault(a => a.Name == "Charisma");
            if (attribute is not null)
            {
                for (int i = 1; i <= attribute.Value; i++)
                {
                    dict.Add($"charisma_{i}", "true");
                }
            }

            attribute = Attributes.FirstOrDefault(a => a.Name == "Manipulation");
            if (attribute is not null)
            {
                for (int i = 1; i <= attribute.Value; i++)
                {
                    dict.Add($"manipulation_{i}", "true");
                }
            }

            attribute = Attributes.FirstOrDefault(a => a.Name == "Composure");
            if (attribute is not null)
            {
                for (int i = 1; i <= attribute.Value; i++)
                {
                    dict.Add($"composure_{i}", "true");
                }
            }

            attribute = Attributes.FirstOrDefault(a => a.Name == "Intelligence");
            if (attribute is not null)
            {
                for (int i = 1; i <= attribute.Value; i++)
                {
                    dict.Add($"intelligence_{i}", "true");
                }
            }

            attribute = Attributes.FirstOrDefault(a => a.Name == "Wits");
            if (attribute is not null)
            {
                for (int i = 1; i <= attribute.Value; i++)
                {
                    dict.Add($"wits_{i}", "true");
                }
            }

            attribute = Attributes.FirstOrDefault(a => a.Name == "Resolve");
            if (attribute is not null)
            {
                for (int i = 1; i <= attribute.Value; i++)
                {
                    dict.Add($"resolve_{i}", "true");
                }
            }

            var skill = Skills.FirstOrDefault(a => a.Name == "Athletics");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"athletics_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Brawl");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"brawl_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Craft");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"craft_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Drive");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"drive_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Firearms");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"firearms_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Larceny");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"larceny_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Melee");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"melee_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Stealth");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"stealth_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Survival");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"survival_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Animal Ken");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"animal_ken_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Ettiquite");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"ettiquite_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Insight");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"insight_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Intimidation");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"intimidation_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Leadership");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"leadership_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Performance");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"performance_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Persuasion");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"persuasion_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Streetwise");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"streetwise_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Subterfuge");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"subterfuge_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Academics");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"academics_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Awareness");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"awareness_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Finance");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"finance_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Investigation");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"investigation_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Medicine");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"medicine_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Occult");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"occult_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Politics");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"politics_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Science");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"science_{i}", "true");
                }
            }

            skill = Skills.FirstOrDefault(a => a.Name == "Technology");
            if (skill is not null)
            {
                for (int i = 1; i <= skill.Value; i++)
                {
                    dict.Add($"technology_{i}", "true");
                }
            }

            foreach (var specialty in Specialties)
            {
                dict.Add($"{specialty.Skill.ToLower()}_specialty", specialty.Name);
            }

            for (int i = 1; i <= Health; i++)
            {
                dict.Add($"health_{i}", "true");
            }

            for (int i = 1; i <= Willpower; i++)
            {
                dict.Add($"willpower_{i}", "true");
            }

            for (int i = 1; i <= Humanity; i++)
            {
                dict.Add($"humanity_{i}", "true");
            }

            for (int i = 1; i <= Hunger; i++)
            {
                dict.Add($"hunger_{i}", "true");
            }

            for (int i = 1; i <= BloodPotency; i++)
            {
                dict.Add($"blood_potency_{i}", "true");
            }

            int discIndex = 1;
            foreach (var discipline in Disciplines.Where(d => d.Value > 0))
            {
                dict.Add($"discipline_{discIndex}_name", discipline.Name);
                
                for (int i = 1; i <= discipline.Value; i++)
                {
                    dict.Add($"discipline_{discIndex}_dot_{i}", "true");
                }

                int powerIndex = 1;
                foreach (var power in Powers.Where(p => p.Discipline == discipline.Name))
                {
                    dict.Add($"discipline_{discIndex}_power_{powerIndex}", power.Name);
                    powerIndex++;
                }

                discIndex++;

                //The sheet only supports up to six disciplines
                if (discIndex > 6) break;
            }

            int backgroundIndex = 1;
            foreach (var background in Backgrounds)
            {
                dict.Add($"background_{backgroundIndex}_name", background.Name);

                for (int i = 1; i <= background.Value; i++)
                {
                    dict.Add($"background_{backgroundIndex}_dot_{i}", "true");
                }

                backgroundIndex++;

                if (backgroundIndex > 9) break;
            }

            int meritIndex = 1;
            foreach (var merit in Merits)
            {
                dict.Add($"merit_{meritIndex}_name", merit.Name);

                for (int i = 1; i <= merit.Value; i++)
                {
                    dict.Add($"merit_{meritIndex}_dot_{i}", "true");
                }

                meritIndex++;

                if (meritIndex > 7) break;
            }

            int flawIndex = 1;
            foreach (var flaw in Flaws)
            {
                dict.Add($"flaw_{flawIndex}_name", flaw.Name);

                for (int i = 1; i <= flaw.Value; i++)
                {
                    dict.Add($"flaw_{flawIndex}_dot_{i}", "true");
                }

                flawIndex++;

                if (flawIndex > 7) break;
            }

            List<string> tenetsArray = ChronicleTenets.DivideStringIntoWordArray(20);
            for (int i = 0; i < tenetsArray.Count; i++)
            {
                if (i <= 5)
                {
                    dict.Add($"chronicle_tenet_{i + 1}", tenetsArray[i]);
                }
            }

            List<string> touchstoneArray = TouchstonesConvictions.DivideStringIntoWordArray(20);
            for (int i = 0; i < touchstoneArray.Count; i++)
            {
                if (i <= 5)
                {
                    dict.Add($"touchstone_conviction{i + 1}", touchstoneArray[i]);
                }
            }

            List<string> baneArray = Clan.Bane.DivideStringIntoWordArray(20);
            for (int i = 0; i < baneArray.Count; i++)
            {
                if (i <= 5)
                {
                    dict.Add($"clan_bane_{i + 1}", baneArray[i]);
                }
            }

            List<string> notesArray = Notes.DivideStringIntoWordArray(20);
            for (int i = 0; i < notesArray.Count; i++)
            {
                if (i <= 12)
                {
                    dict.Add($"notes_line_{i + 1}", notesArray[i]);
                }
            }

            List<string> appearanceArray = Appearance.DivideStringIntoWordArray(20);
            for (int i = 0; i < appearanceArray.Count; i++)
            {
                if (i <= 5)
                {
                    dict.Add($"bio_line_{i + 5}", appearanceArray[i]);
                }
            }

            List<string> disFeatArray = DistinguishingFeatures.DivideStringIntoWordArray(20);
            for (int i = 0; i < disFeatArray.Count; i++)
            {
                if (i <= 3)
                {
                    dict.Add($"bio_line_{i + 11}", disFeatArray[i]);
                }
            }

            List<string> historyArray = History.DivideStringIntoWordArray(20);
            for (int i = 0; i < historyArray.Count; i++)
            {
                if (i <= 13)
                {
                    dict.Add($"bio_line_{i + 16}", historyArray[i]);
                }
            }

            for (int i = 1; i < Weapons.Count; i++)
            {
                if (i <= 6) //Maximum lines on character sheet
                {
                    dict.Add($"weapon_{i}_name", Weapons[i - 1].Name);
                    dict.Add($"weapon_{i}_damage", Weapons[i - 1].Damage.ToString());
                }
            }

            for (int i = 1; i < Gears.Count; i++)
            {
                if (i <= 6) //Maximum lines on character sheet
                {
                    dict.Add($"possession_line_{i}", Gears[i - 1].Name);
                }
            }

            return PDFSchema.Generate(dict, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/VampireTheMasquerade_Character_Sheet.pdf");
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
                Details = $"{Clan.Name} | {Predator.Name}",
                CharacterSheet = CharacterSheet
            };
        }
    }
}
