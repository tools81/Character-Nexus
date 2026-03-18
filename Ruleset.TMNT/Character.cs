using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public List<Psionic> Psionics { get; set; }
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
            var dict = new Dictionary<string, string>
            {
                { "Name",       Name ?? "" },
                { "Age",        Age > 0 ? Age.ToString() : "" },
                { "Gender",     Gender ?? "" },
                { "Weight",     Weight ?? "" },
                { "Height",     Height ?? "" },
                { "Level",      Level.ToString() },
                { "Exp",        XP.ToString() },
                { "Hit.Points", HitPoints.ToString() },
                { "SDC",        SDC.ToString() },
                { "Animal",     Animal?.Name ?? "" },
                { "Alignment",  Alignment?.Name ?? "" },
                { "Origin.Mutant",     Mutation?.Name ?? "" },
                { "Origin.Animal.Size", Animal?.Size.ToString() ?? "" },
                { "Mutant.Form.Size",  Size ?? "" },
            };

            if (!string.IsNullOrEmpty(Disposition))
                dict["Disposition.0"] = Disposition;

            if (!string.IsNullOrEmpty(Notes))
                dict["Ch.Notes.0"] = Notes;

            if (!string.IsNullOrEmpty(Origin))
                dict["Build.Notes.0"] = Origin;

            // Attributes
            var attrMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "IQ",  "Intelligence" },
                { "ME",  "Mental.Endurance" },
                { "MA",  "Mental.Affinity" },
                { "PS",  "Physical.Strength" },
                { "PP",  "Physical.Prowess" },
                { "PE",  "Physical.Endurance" },
                { "PB",  "Physical.Beauty" },
                { "Spd", "Speed" },
            };
            if (Attributes != null)
            {
                foreach (var attr in Attributes)
                {
                    if (attrMap.TryGetValue(attr.Name, out var pdfField))
                        dict[pdfField] = attr.Value.ToString();
                }
            }

            // Animal abilities
            if (Biped != null)
            {
                dict["Human.Features.Biped.0"]      = Biped.Name;
                dict["Mutant.Biped"]                 = Biped.Name;
                dict["BioE.Cost.Human.Features.Biped"] = Biped.Cost.ToString();
            }
            if (Hand != null)
            {
                dict["Human.Features.Hands.0"]      = Hand.Name;
                dict["Mutant.Hands"]                 = Hand.Name;
                dict["BioE.Cost.Human.Features.Hands"] = Hand.Cost.ToString();
            }
            if (Look != null)
            {
                dict["Human.Features.Looks.0"]      = Look.Name;
                dict["Mutant.Looks"]                 = Look.Name;
                dict["BioE.Cost.Human.Features.Looks"] = Look.Cost.ToString();
            }
            if (Speech != null)
            {
                dict["Human.Features.Speech.0"]       = Speech.Name;
                dict["Mutant.Speech"]                  = Speech.Name;
                dict["BioE.Cost.Human.Features.Speech"] = Speech.Cost.ToString();
            }
            if (NaturalWeapon != null)
            {
                dict["Mutant.Natural.Weapons"] = NaturalWeapon.Name;
                dict["Mutant.Natural.Damage"]  = NaturalWeapon.Description ?? "";
                var nwLower = NaturalWeapon.Name.ToLower();
                if (nwLower.Contains("claw"))
                    dict["BioE.Cost.Claws.Abilities"] = NaturalWeapon.Cost.ToString();
                else if (nwLower.Contains("teeth") || nwLower.Contains("bite"))
                    dict["BioE.Cost.Teeth.Abilities"] = NaturalWeapon.Cost.ToString();
            }

            // Skills — split into H2H, WP, and regular
            if (Skills != null)
            {
                var h2hSkill = Skills.FirstOrDefault(s =>
                    s.Name.StartsWith("Hand to Hand", StringComparison.OrdinalIgnoreCase) && s.Value > 0);
                var wpSkills = Skills
                    .Where(s => s.Name.StartsWith("WP ", StringComparison.OrdinalIgnoreCase) && s.Value > 0)
                    .ToList();
                var regularSkills = Skills
                    .Where(s => !s.Name.StartsWith("WP ", StringComparison.OrdinalIgnoreCase)
                             && !s.Name.StartsWith("Hand to Hand", StringComparison.OrdinalIgnoreCase)
                             && s.Value > 0)
                    .ToList();

                if (h2hSkill != null)
                    dict["Combat.Style"] = h2hSkill.Name;

                for (int i = 0; i < regularSkills.Count && i < 12; i++)
                {
                    dict[$"Scholastic.Skills.{i}"]          = regularSkills[i].Name;
                    dict[$"Percent.Secondary.Skills.{i}"]   = regularSkills[i].Value.ToString();
                }

                for (int i = 0; i < wpSkills.Count && i < 6; i++)
                    dict[$"Weapon.Proficiency.{i}"] = wpSkills[i].Name;
            }

            // Psionics (8 slots)
            if (Psionics != null)
            {
                for (int i = 0; i < Psionics.Count && i < 8; i++)
                {
                    dict[$"Psionic.Spell.Name.{i}"]         = Psionics[i].Name;
                    dict[$"Psionic.Spell.Effect.{i}"]       = Psionics[i].Description ?? "";
                    dict[$"Psionic.Spell.Range.{i}"]        = Psionics[i].Range ?? "";
                    dict[$"Psionic.Spell.Saving.Throw.{i}"] = Psionics[i].SavingThrow ?? "";
                    dict[$"Spell.Duration.{i}"]             = Psionics[i].Duration ?? "";
                }
            }

            // Weapons (5 slots, indexed 0–4)
            if (Weapons != null)
            {
                for (int i = 0; i < Weapons.Count && i < 5; i++)
                {
                    dict[$"Type.Weapon.{i}"] = Weapons[i].Name;
                    dict[$"W.Damage.{i}"]    = Weapons[i].Damage ?? "";
                    dict[$"W.Range.{i}"]     = Weapons[i].Range ?? "";
                    dict[$"W.Notes.{i}"]     = Weapons[i].Description ?? "";
                }
            }

            // Equipment (4 slots, indexed 1–4)
            if (Equipments != null)
            {
                for (int i = 0; i < Equipments.Count && i < 4; i++)
                    dict[$"Equipment.Valuables.{i + 1}"] = Equipments[i].Name;
            }

            // Vehicle
            if (Vehicle != null)
                dict["Equipment.Valuables.4"] = Vehicle.Name;

            // Armor
            if (Armor != null)
            {
                dict["Armor.Type"]   = Armor.Name;
                dict["Armor.Rating"] = Armor.AR.ToString();
                dict["Armor.SDC"]    = Armor.SDC.ToString();
                dict["Armor.Weight"] = Armor.Weight ?? "";
            }

            return PDFSchema.Generate(dict,
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/TMNT_Character_Sheet.pdf");
        }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                Image = Image,
                Level = Level.ToString(),
                LevelName = "Level",
                Details = $"{Animal?.Name} | {Alignment?.Name}",
                CharacterSheet = CharacterSheet
            };
        }
    }
}