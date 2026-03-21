using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Utility;

namespace CallOfCthulhu
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Era Era { get; set; }
        public string Birthplace { get; set; }
        public string Pronoun { get; set; }
        public string Residence { get; set; }
        public string Story { get; set; }
        public string Description { get; set; }
        public string Ideology { get; set; }
        public string SignificantPeople { get; set; }
        public string MeaningfulLocations { get; set; }
        public string TreasuredPosessions { get; set; }
        public string Traits { get; set; }
        public string Scars { get; set; }
        public Occupation Occupation { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Phobia> Phobias { get; set; }
        public List<Mania> Manias { get; set; }
        public List<Spell> Spells { get; set; }
        public string Encounters { get; set; }
        public string CreditRating { get; set; }
        public string SpendingLevel { get; set; }
        public int Cash { get; set; }
        public string Assets { get; set; }
        public string Notes { get; set; }
        public int DamageBonus { get; set; }
        public int Build { get; set; }
        public int HitPoints { get; set; }
        public int Movement { get; set; }
        public int SanityPoints { get; set; }
        public int MagicPoints { get; set; }
        public List<Equipment> Equipments { get; set; }
        public List<Weapon> Weapons { get; set; }

        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }

        public string CharacterSheet { get; set; }

        public byte[] BuildCharacterSheet()
        {
            var is1920s = Era?.Name.Equals("1920s", StringComparison.OrdinalIgnoreCase) == true;
            var pdfFile = is1920s
                ? "Call_Of_Cthulhu_1920s_Character_Sheet.pdf"
                : "Call_Of_Cthulhu_Modern_Character_Sheet.pdf";
            var pdfPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/" + pdfFile;

            var dict = new Dictionary<string, string>
            {
                { "Investigators_Name", Name ?? "" },
                { "Birthplace",         Birthplace ?? "" },
                { "Pronouns",           Pronoun ?? "" },
                { "Occupation",         Occupation?.Name ?? "" },
                { "Residence",          Residence ?? "" },
                { "StartingHP",         HitPoints.ToString() },
                { "CurrentHP",          HitPoints.ToString() },
                { "StartingMagic",      MagicPoints.ToString() },
                { "CurrentMagic",       MagicPoints.ToString() },
                { "StartingSanity",     SanityPoints.ToString() },
                { "CurrentSanity",      SanityPoints.ToString() },
                { "MOV",                Movement.ToString() },
                { "Build",              Build.ToString() },
                { "DamageBonus",        DamageBonus.ToString() },
                { "MyStory",            Story ?? "" },
                { "PersonalDescription",Description ?? "" },
                { "Traits",             Traits ?? "" },
                { "Ideology/Beliefs",   Ideology ?? "" },
                { "Injuries",           Scars ?? "" },
                { "Significant People", SignificantPeople ?? "" },
                { "Locations",          MeaningfulLocations ?? "" },
                { "Possessions",        TreasuredPosessions ?? "" },
                { "Encounters",         Encounters ?? "" },
                { "SpendingLevel",      SpendingLevel ?? "" },
                { "Cash",               Cash.ToString() },
                { "Assets1",            Assets ?? "" },
                { "ExtraText",          Notes ?? "" },
            };

            // Phobias / Manias
            var phobiaManiaLines = new List<string>();
            if (Phobias != null) phobiaManiaLines.AddRange(Phobias.Select(p => p.Name));
            if (Manias  != null) phobiaManiaLines.AddRange(Manias.Select(m => m.Name));
            dict["Phobias/Manias"] = string.Join(", ", phobiaManiaLines);

            // Spells
            if (Spells != null)
                dict["Tomes/Spells"] = string.Join(", ", Spells.Select(s => s.Name));

            // Equipment
            if (Equipments != null && Equipments.Count > 0)
            {
                var equipNames = Equipments.Select(e => e.Name).ToList();
                var half = (int)Math.Ceiling(equipNames.Count / 2.0);
                dict["Gear/Possessions"]  = string.Join(", ", equipNames.Take(half));
                dict["Gear/Possessions1"] = string.Join(", ", equipNames.Skip(half));
            }

            // Skills
            // Standard 1-to-1 mappings
            var skillMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Accounting",        "Skill_Accounting"       },
                { "Anthropology",      "Skill_Anthropology"     },
                { "Appraise",          "Skill_Appraise"         },
                { "Archaeology",       "Skill_Archaeology"      },
                { "Charm",             "Skill_Charm"            },
                { "Climb",             "Skill_Climb"            },
                { "Credit Rating",     "Skill_Credit"           },
                { "Cthulhu Mythos",    "Skill_Cthulhu"          },
                { "Disguise",          "Skill_Disguise"         },
                { "Dodge",             "Skill_Dodge"            },
                { "Drive Auto",        "Skill_Drive"            },
                { "Electrical Repair", "Skill_ElecRepair"       },
                { "Fast Talk",         "Skill_FastTalk"         },
                { "Brawl",             "Skill_Fighting"         },
                { "Handgun",           "Skill_FireArmsHandguns" },
                { "Rifle/Shotgun",     "Skill_FireArmsRifles"   },
                { "First Aid",         "Skill_FirstAid"         },
                { "History",           "Skill_History"          },
                { "Intimidate",        "Skill_Intimidate"       },
                { "Jump",              "Skill_Jump"             },
                { "Language (Own)",    "Skill_OwnLanguage"      },
                { "Law",               "Skill_Law"              },
                { "Library Use",       "Skill_LibraryUse"       },
                { "Listen",            "Skill_Listen"           },
                { "Locksmith",         "Skill_Locksmith"        },
                { "Mechanical Repair", "Skill_MechRepair"       },
                { "Medicine",          "Skill_Medicine"         },
                { "Natural World",     "Skill_NaturalWorld"     },
                { "Navigate",          "Skill_Navigate"         },
                { "Occult",            "Skill_Occult"           },
                { "Persuade",          "Skill_Persuade"         },
                { "Pilot",             "Skill_Pilot"            },
                { "Psychoanalysis",    "Skill_Psychoanalysis"   },
                { "Psychology",        "Skill_Psychology"       },
                { "Ride",              "Skill_Ride"             },
                { "Sleight of Hand",   "Skill_Sleight"          },
                { "Spot Hidden",       "Skill_SpotHidden"       },
                { "Stealth",           "Skill_Stealth"          },
                { "Survival",          "Skill_Survival"         },
                { "Swim",              "Skill_Swim"             },
                { "Throw",             "Skill_Throw"            },
                { "Track",             "Skill_Track"            },
            };

            if (!is1920s)
            {
                skillMap["Computer Use"] = "Skill_Computer";
                skillMap["Electronics"]  = "Skill_Electronic";
            }

            // Specialty buckets
            var fightingSpecialties = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { "Axe", "Bow", "Chainsaw", "Flail", "Garrote", "Spear", "Sword", "Whip" };
            var firearmsSpecialties = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { "Artillery", "Flamethrower", "Heavy Weapons", "Machine Gun", "Shotgun", "Submachine Gun" };
            var scienceSkills = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { "Astronomy", "Biology", "Botany", "Chemistry", "Geology", "Mathematics", "Meteorology", "Physics", "Zoology" };

            string[] langOtherSlots    = { "Skill_OtherLanguage",    "Skill_OtherLanguage1",    "Skill_OtherLanguage2"    };
            string[] langOtherDefSlots = { "SkillDef_OtherLanguage", "SkillDef_OtherLanguage1", "SkillDef_OtherLanguage2" };

            string[] scienceSlots, scienceDefSlots;
            if (is1920s)
            {
                scienceSlots    = new[] { "Skill_Science1",    "Skill_Science2",    "Skill_Science3"    };
                scienceDefSlots = new[] { "SkillDef_Science1", "SkillDef_Science2", "SkillDef_Science3" };
            }
            else
            {
                scienceSlots    = new[] { "Skill_Science",    "Skill_Science1",    "Skill_Science2"    };
                scienceDefSlots = new[] { "SkillDef_Science", "SkillDef_Science1", "SkillDef_Science2" };
            }

            int fightingIdx  = 0;
            int firearmsIdx  = 0;
            int langOtherIdx = 0;
            int scienceIdx   = 0;
            int customIdx    = 0;
            int maxFighting  = is1920s ? 2 : 1;
            int maxCustom    = is1920s ? 4 : 3;

            if (Skills != null)
            {
                foreach (var skill in Skills)
                {
                    if (skillMap.TryGetValue(skill.Name, out var fieldBase))
                    {
                        SetSkillFields(dict, fieldBase, skill.Value);
                        if (string.Equals(skill.Name, "Dodge", StringComparison.OrdinalIgnoreCase))
                            SetSkillFields(dict, "Dodge_Copy", skill.Value);
                    }
                    else if (string.Equals(skill.Name, "Language (Other)", StringComparison.OrdinalIgnoreCase))
                    {
                        if (langOtherIdx < langOtherSlots.Length)
                        {
                            dict[langOtherDefSlots[langOtherIdx]] = "Other Language";
                            SetSkillFields(dict, langOtherSlots[langOtherIdx], skill.Value);
                            langOtherIdx++;
                        }
                    }
                    else if (string.Equals(skill.Name, "Art and Craft", StringComparison.OrdinalIgnoreCase))
                    {
                        SetSkillFields(dict, "Skill_ArtCraft1", skill.Value);
                    }
                    else if (scienceSkills.Contains(skill.Name))
                    {
                        if (scienceIdx < scienceSlots.Length)
                        {
                            dict[scienceDefSlots[scienceIdx]] = skill.Name;
                            SetSkillFields(dict, scienceSlots[scienceIdx], skill.Value);
                            scienceIdx++;
                        }
                    }
                    else if (fightingSpecialties.Contains(skill.Name))
                    {
                        if (fightingIdx < maxFighting)
                        {
                            var slot = fightingIdx + 1;
                            dict[$"SkillDef_Fighting{slot}"] = skill.Name;
                            SetSkillFields(dict, $"Skill_Fighting{slot}", skill.Value);
                            fightingIdx++;
                        }
                    }
                    else if (firearmsSpecialties.Contains(skill.Name))
                    {
                        if (firearmsIdx < 1)
                        {
                            dict["SkillDef_Firearms"] = skill.Name;
                            SetSkillFields(dict, "Skill_Firearms", skill.Value);
                            firearmsIdx++;
                        }
                    }
                    else if (customIdx < maxCustom)
                    {
                        var slot = customIdx + 1;
                        dict[$"SkillDef_Custom{slot}"] = skill.Name;
                        SetSkillFields(dict, $"Skill_Custom{slot}", skill.Value);
                        customIdx++;
                    }
                }
            }

            // Weapons (3 slots; slot 0 is the unarmed/brawl row)
            var brawl = Skills?.FirstOrDefault(s => s.Name.Equals("Brawl", StringComparison.OrdinalIgnoreCase));
            if (brawl != null)
            {
                dict["Weapon_Regular0"] = brawl.Value.ToString();
                dict["Weapon_Hard0"]    = (brawl.Value / 2).ToString();
                dict["Weapon_Extreme0"] = (brawl.Value / 5).ToString();
            }

            if (Weapons != null)
            {
                for (int i = 0; i < Weapons.Count && i < 3; i++)
                {
                    var w    = Weapons[i];
                    var slot = i + 1;
                    dict[$"Weapon_Name{slot}"]    = w.Name ?? "";
                    dict[$"Weapon_Damage{slot}"]  = w.Damage ?? "";
                    dict[$"Weapon_Range{slot}"]   = w.Range ?? "";
                    dict[$"Weapon_Ammo{slot}"]    = w.Magazine ?? "";
                    dict[$"Weapon_Malf{slot}"]    = w.Malfunction > 0 ? w.Malfunction.ToString() : "";
                    dict[$"Weapon_Attacks{slot}"] = w.Uses ?? "";

                    // Look up the associated skill percentage
                    var weaponSkill = Skills?.FirstOrDefault(s =>
                        !string.IsNullOrEmpty(w.Skill) &&
                        s.Name.Contains(w.Skill, StringComparison.OrdinalIgnoreCase));
                    if (weaponSkill != null)
                    {
                        dict[$"Weapon_Regular{slot}"] = weaponSkill.Value.ToString();
                        dict[$"Weapon_Hard{slot}"]    = (weaponSkill.Value / 2).ToString();
                        dict[$"Weapon_Extreme{slot}"] = (weaponSkill.Value / 5).ToString();
                    }
                }
            }

            return PDFSchema.Generate(dict, pdfPath);
        }

        private static void SetSkillFields(Dictionary<string, string> dict, string fieldBase, int value)
        {
            dict[fieldBase]              = value.ToString();
            dict[fieldBase + "_half"]    = (value / 2).ToString();
            dict[fieldBase + "_fifth"]   = (value / 5).ToString();
        }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment() {
                Id = Id,
                Name = Name,
                Image = Image,
                Level = Era.Name,
                LevelName = "Era",
                Details = $"{Occupation.Name}",
                CharacterSheet = CharacterSheet
            };
        }
    }
}