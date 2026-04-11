using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace EverydayHeroes
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Level { get; set; }
        public string Languages { get; set; }
        public string Motivations { get; set; }
        public string Attachments { get; set; }
        public string Beliefs { get; set; }
        public string Ancestry { get; set; }
        public string Quirks { get; set; }
        public string Virtues { get; set; }
        public string Flaws { get; set; }
        public string Role { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string Hair { get; set; }
        public string Skin { get; set; }
        public string Eyes { get; set; }
        public string Age { get; set; }
        public string MaritalStatus { get; set; }
        public string Pronouns { get; set; }
        public string Biography { get; set; }
        public string Notes { get; set; }
        public Archetype Archetype { get; set; }
        public Class Class { get; set; }
        public Background Background { get; set; }
        public Profession Profession { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Talent> Talents { get; set; }
        public List<Plan> Plans { get; set; }
        public List<Trick> Tricks { get; set; }
        public List<Pack> Packs { get; set; }
        public List<Feat> Feats { get; set; }
        public List<Weapon> Weapons { get; set; }
        public List<Item> Items { get; set; }
        public List<Armor> Armors { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<string> EquipmentProficiency { get; set; }
        public List<string> SavingThrowProficiency { get; set; }
        public int Speed { get => 30; }
        public string HitDice { get; set; }
        public int HitPointBase { get; set; }
        public int HitPoints { get; set; }
        public int HitPointModifier { get; set; }
        public int DefenseBonus { get; set; }
        public string DefenseModifier { get; set; }
        public int Defense { get; set; }
        public int Initiative { get; set; }
        public bool HasDamageReduction { get; set; }
        public int DamageReduction { get; set; }
        public int ProficiencyBonus { get; set; }
        public int PassivePerception { get; set; }  
        public int WealthLevel { get; set; } 
        public int GeniusPoints { get; set; }    
        public int FocusPoints { get; set; }
        public int InfluencePoints { get; set; }
        public int LuckPoints { get; set; }
        public bool Inspiration { get; set; }
        public Choice Choice { get; set; } = new Choice();
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
                Details = $"{Archetype.Name} | {Class.Name}",
                CharacterSheet = CharacterSheet
            };
        }

        public byte[] BuildCharacterSheet()
        {
            var dict = new Dictionary<string, string>();

            // Page 0 – Header
            dict["Hero Name"]  = Name ?? "";
            dict["Class"]      = Class?.Name ?? "";
            dict["Archetype"]  = Archetype?.Name ?? "";
            dict["Background"] = Background?.Name ?? "";
            dict["Level"]      = Level.ToString();
            dict["Profession"] = Profession?.Name ?? "";
            dict["Wealth"]     = WealthLevel.ToString();
            dict["Speed"]      = Speed.ToString();
            dict["Languages"]  = Languages ?? "";

            // Attributes + Saving Throws
            var saveCheckbox = new Dictionary<string, string>
            {
                { "Strength",     "STR Save" },
                { "Dexterity",    "DEX Save" },
                { "Constitution", "CON Save" },
                { "Intelligence", "INT Save" },
                { "Wisdom",       "Wis Save" },
                { "Charisma",     "CHA Save" },
            };
            var attrAbbrev = new Dictionary<string, string>
            {
                { "Strength",     "STR" },
                { "Dexterity",    "DEX" },
                { "Constitution", "CON" },
                { "Intelligence", "INT" },
                { "Wisdom",       "WIS" },
                { "Charisma",     "CHA" },
            };
            foreach (var attr in Attributes ?? new List<Attribute>())
            {
                if (!attrAbbrev.TryGetValue(attr.Name, out var abbrev)) continue;
                bool saveProf = (SavingThrowProficiency ?? new List<string>()).Contains(attr.Name);
                dict[abbrev]                  = attr.Value.ToString();
                dict[$"{abbrev} Bonus"]       = FormatModifier(attr.Modifier);
                dict[saveCheckbox[attr.Name]] = saveProf ? "Yes" : "";
                dict[$"{abbrev} Save #"]      = FormatModifier(attr.Modifier + (saveProf ? ProficiencyBonus : 0));
            }

            // Skills
            var skillFields = new Dictionary<string, (string Mod, string Prof, string Exp)>
            {
                { "Acrobatics",       ("Acro #",    "AcrobatP",       "AcrobatE")       },
                { "Arts and Crafts",  ("Arts #",    "ArtsCP",         "ArtsCE")         },
                { "Athletics",        ("Athl #",    "AthleticsP",     "AlthleticsE")    },
                { "Computers",        ("Comp #",    "ComputersP",     "ComputersE")     },
                { "Deception",        ("Dece #",    "DecpetionP",     "DeceptionE")     },
                { "Endurance",        ("Endu #",    "EnduranceP",     "EnduranceE")     },
                { "Insight",          ("Insi #",    "InsightP",       "InsightE")       },
                { "Intimidation",     ("Inti #",    "IntimidationP",  "IndimidationE")  },
                { "Investigation",    ("Inve #",    "InvestigationP", "InvestigationE") },
                { "Mechanics",        ("Mech #",    "MechanicsP",     "MechanicsE")     },
                { "Medicine",         ("Medi #",    "MedicineP",      "MedicineE")      },
                { "Natural Sciences", ("Natu #",    "NaturalP",       "NaturalE")       },
                { "Perception",       ("Prec #",    "PreceptionP",    "PreceptionE")    },
                { "Performance",      ("Perf #",    "PerformanceP",   "PerformanceE")   },
                { "Persuasion",       ("pers #",    "PersuastionP",   "PersuastionE")   },
                { "Security",         ("Secu #",    "SecurityP",      "SecurityE")      },
                { "Sleight of Hand",  ("Sleit #",   "SleightP",       "SleightE")       },
                { "Social Sciences",  ("Soci #",    "SocSciP",        "SocSciE")        },
                { "Stealth",          ("Stealth #", "StealthP",       "StealthE")       },
                { "Streetwise",       ("Street #",  "StreetwiseP",    "StreetwiseE")    },
                { "Survival",         ("Surv #",    "SurvivalP",      "SurvivalE")      },
                { "Vehicles",         ("Vehic #",   "VehiclesP",      "VehiclesE")      },
            };
            foreach (var skill in Skills ?? new List<Skill>())
            {
                if (!skillFields.TryGetValue(skill.Name, out var f)) continue;
                var attr = Attributes?.FirstOrDefault(a => a.Name == skill.AbilityModifier);
                int mod = (attr?.Modifier ?? 0)
                    + (skill.Proficient ? ProficiencyBonus : 0)
                    + (skill.Expertise  ? ProficiencyBonus : 0);
                dict[f.Mod]  = FormatModifier(mod);
                dict[f.Prof] = skill.Proficient ? "Yes" : "";
                dict[f.Exp]  = skill.Expertise  ? "Yes" : "";
            }

            // Combat stats
            dict["Hit Dice"]           = HitDice ?? "";
            dict["Hit Points"]         = HitPoints.ToString();
            dict["Proficiency Bonus"]  = FormatModifier(ProficiencyBonus);
            dict["Defense"]            = Defense.ToString();
            dict["Initiative"]         = FormatModifier(Initiative);
            dict["Passive Perception"] = PassivePerception.ToString();

            // Resource points
            dict["Genius"]      = GeniusPoints.ToString();
            dict["Focus"]       = FocusPoints.ToString();
            dict["Influence"]   = InfluencePoints.ToString();
            dict["Inspiration"] = Inspiration ? "Yes" : "";

            // Equipment proficiency checkboxes (Basic, Advanced, Military, Historical, Improvised)
            foreach (var ep in EquipmentProficiency ?? new List<string>())
                dict[ep] = "Yes";

            // Weapons (3 slots on page 0)
            var wsfx = new[] { "", "2", "3" };
            for (int i = 0; i < Math.Min(Weapons?.Count ?? 0, 3); i++)
            {
                var w = Weapons[i];
                dict[$"Weapon{i + 1}"]      = w.Name ?? "";
                dict[$"Props{i + 1}"]       = w.Properties != null ? string.Join(", ", w.Properties) : "";
                dict[$"DMG{wsfx[i]}"]       = w.Damage ?? "";
                dict[$"PV{wsfx[i]}"]        = w.Penetration.ToString();
                if (!string.IsNullOrEmpty(w.Range))
                {
                    var rp = w.Range.Split('/');
                    dict[$"RangeReg{wsfx[i]}"]  = rp[0].Trim();
                    dict[$"RangeLong{wsfx[i]}"] = rp.Length > 1 ? rp[1].Split(' ')[0].Trim() : "";
                }
            }

            // Armor
            if (Armors?.Count > 0)
            {
                var armor = Armors[0];
                dict["Armor"]      = armor.Name ?? "";
                dict["ArmorProps"] = armor.Properties != null ? string.Join(", ", armor.Properties) : "";
                dict["ArmorAV"]    = armor.Value.ToString();
            }

            // Vehicle summary (page 0)
            if (Vehicles?.Count > 0)
            {
                var v = Vehicles[0];
                dict["Vehicle"]      = v.Name ?? "";
                dict["VehicleProps"] = v.Properties != null ? string.Join(", ", v.Properties) : "";
                dict["TopSpd"]       = v.Speed ?? "";
                dict["VehicleAV"]    = v.ArmorValue.ToString();
                dict["VehicleSTR"]   = v.Strength.ToString();
                dict["VehicleDEX"]   = v.Dexterity.ToString();
                dict["VehicleCON"]   = v.Constitution.ToString();
            }

            // Talents & Feats summary lines on page 0 (T&F01–T&F12)
            var featureNames = new List<string>();
            if (Talents != null) featureNames.AddRange(Talents.Select(t => t.Name));
            if (Feats   != null) featureNames.AddRange(Feats.Select(f => f.Name));
            for (int i = 0; i < Math.Min(featureNames.Count, 12); i++)
                dict[$"T&F{i + 1:D2}"] = featureNames[i];

            // Page 1 – Talents & Feats full text
            var tfSb = new System.Text.StringBuilder();
            if (Talents != null) foreach (var t in Talents) tfSb.AppendLine($"{t.Name}: {t.Description}");
            if (Feats   != null) foreach (var f in Feats)   tfSb.AppendLine($"{f.Name}: {f.Description}");
            if (tfSb.Length > 0) dict["Talents & Feats"] = tfSb.ToString();

            // Page 1 – Plans & Tricks full text
            var ptSb = new System.Text.StringBuilder();
            if (Plans  != null) foreach (var p in Plans)  ptSb.AppendLine($"{p.Name}: {p.Description}");
            if (Tricks != null) foreach (var t in Tricks) ptSb.AppendLine($"{t.Name}: {t.Description}");
            if (ptSb.Length > 0) dict["Plans & Tricks"] = ptSb.ToString();

            // Page 1 – Carried Equipment (items + packs, up to 16 rows)
            var carried = new List<(string name, string bulk)>();
            if (Items != null) carried.AddRange(Items.Select(i => (i.Name ?? "", i.bulk.ToString())));
            if (Packs != null) carried.AddRange(Packs.Select(p => (p.Name ?? "", "")));
            for (int i = 0; i < Math.Min(carried.Count, 16); i++)
            {
                dict[$"Carried equipmentRow{i}"] = carried[i].name;
                dict[$"bulkRow{i}"]              = carried[i].bulk;
            }

            // Page 2 – Bio / Description
            dict["Name"]           = Name ?? "";
            dict["Weight"]         = Weight ?? "";
            dict["Height"]         = Height ?? "";
            dict["Hair"]           = Hair ?? "";
            dict["Skin"]           = Skin ?? "";
            dict["Eyes"]           = Eyes ?? "";
            dict["Age"]            = Age ?? "";
            dict["Marital Status"] = MaritalStatus ?? "";
            dict["Pronouns"]       = Pronouns ?? "";
            dict["Motivations"]    = Motivations ?? "";
            dict["Attachments"]    = Attachments ?? "";
            dict["Beliefs"]        = Beliefs ?? "";
            dict["Ancestry"]       = Ancestry ?? "";
            dict["Quirks"]         = Quirks ?? "";
            dict["Virtues"]        = Virtues ?? "";
            dict["Flaws"]          = Flaws ?? "";
            dict["Role"]           = Role ?? "";
            dict["Biography"]      = Biography ?? "";
            dict["Notes"]          = Notes ?? "";

            // Page 4 – Vehicle detail
            if (Vehicles?.Count > 0)
            {
                var v = Vehicles[0];
                dict["Vehicle Name"]       = v.Name ?? "";
                dict["Vehicle STR"]        = v.Strength.ToString();
                dict["Vehicle DEX"]        = v.Dexterity.ToString();
                dict["Vehicle CON"]        = v.Constitution.ToString();
                dict["Vehicle AV"]         = v.ArmorValue.ToString();
                dict["Vehicle Top Speed"]  = v.Speed ?? "";
                dict["Vehicle PAX"]        = v.Passengers ?? "";
                dict["Vehicle Properties"] = v.Properties != null ? string.Join(", ", v.Properties) : "";
            }

            return PDFSchema.Generate(dict, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Everyday_Heroes_Character_Sheet.pdf");
        }

        private static string FormatModifier(int mod)
        {
            return mod >= 0 ? $"+{mod}" : mod.ToString();
        }
    }
}
