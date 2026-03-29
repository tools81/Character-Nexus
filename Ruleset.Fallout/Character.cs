using System.Reflection;
using System.Text.RegularExpressions;
using Utility;

namespace Fallout
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string Image { get; set; } = string.Empty;
        public int CarryWeight { get; set; }
        public int DamageResistance { get; set; }
        public int Defense { get; set; }
        public int Initiative { get; set; }
        public int HealthPoints { get; set; }
        public int MeleeDamage { get; set; }
        public int Level { get; set; }
        public int Caps { get; set; }
        public Origin Origin { get; set; }
        public Trait Trait { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Trait> Traits { get; set; }
        public List<Perk> Perks { get; set; }
        public Pack Pack { get; set; }
        public List<Clothing> Clothings { get; set; }
        public List<Armor> Armors { get; set; }
        public List<ArmorRobot> RobotArmors { get; set; }
        public List<Consumeable> Consumeables { get; set; }
        public List<Item> Items { get; set; }
        public List<Weapon> Weapons { get; set; }
        public List<Ammo> Ammos { get; set; }

        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }

        public string CharacterSheet { get; set; }

        public byte[] BuildCharacterSheet()
        {
            var resourcesDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/";
            bool isRobot = Origin?.Name == "Mister Handy";
            string pdfPath = isRobot
                ? resourcesDir + "Fallout_Character_Sheet_Robot.pdf"
                : resourcesDir + "Fallout_Character_Sheet_NonRobot.pdf";

            var fields = new Dictionary<string, string>();

            // ── Header ────────────────────────────────────────────────────────────
            fields["character_name"]  = Name;
            fields["level"]           = Level.ToString();
            fields["origin"]          = Origin?.Name ?? string.Empty;
            fields["xp_earned"]       = string.Empty;
            fields["xp_to_nextlevel"] = string.Empty;

            // ── S.P.E.C.I.A.L. ───────────────────────────────────────────────────
            // "strenght" and "Luck" are the actual field names baked into the PDF
            var attrKeys = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Strength"]     = "strenght",
                ["Perception"]   = "perception",
                ["Endurance"]    = "endurance",
                ["Charisma"]     = "charisma",
                ["Intelligence"] = "intelligence",
                ["Agility"]      = "agility",
                ["Luck"]         = "Luck"
            };
            foreach (var attr in Attributes ?? [])
                if (attrKeys.TryGetValue(attr.Name, out var key))
                    fields[key] = attr.Value.ToString();

            // ── Combat ────────────────────────────────────────────────────────────
            fields["melee_damage"]     = MeleeDamage.ToString();
            fields["defense"]          = Defense.ToString();
            fields["initiative"]       = Initiative.ToString();
            fields["luck_points"]      = string.Empty;
            fields["poison_dr"]        = DamageResistance.ToString();
            fields["health_maximum_hp"] = HealthPoints.ToString();
            fields["health_current_hp"] = HealthPoints.ToString();

            // ── Skills ────────────────────────────────────────────────────────────
            var skillKeys = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Athletics"]      = "skill_athletics",
                ["Barter"]         = "skill_barter",
                ["Big Guns"]       = "skill_big-guns",
                ["Energy Weapons"] = "skill_energyweapons",
                ["Explosives"]     = "skill_explosives",
                ["Lockpick"]       = "skill_lockpick",
                ["Medicine"]       = "skill_medicine",
                ["Melee Weapons"]  = "skill_meleeweapons",
                ["Pilot"]          = "skill_pilot",
                ["Repair"]         = "skill_repair",
                ["Science"]        = "skill_science",
                ["Small Guns"]     = "skill_smallguns",
                ["Sneak"]          = "skill_sneak",
                ["Speech"]         = "skill_speech",
                ["Survival"]       = "skill_survival",
                ["Throwing"]       = "skill_throwing",
                ["Unarmed"]        = "skill_unarmed"
            };
            foreach (var skill in Skills ?? [])
                if (skillKeys.TryGetValue(skill.Name, out var key))
                    fields[$"{key}_rank"] = skill.Value.ToString();

            // ── Body-location damage resistance ───────────────────────────────────
            // Both PDFs use identical field names; robot location names are mapped
            // to the same prefixes as their human equivalents.
            if (isRobot)
            {
                var robotLocMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["Optics"]    = "head",
                    ["Main Body"] = "torso",
                    ["Arm 1"]     = "left-arm",
                    ["Arm 2"]     = "right-arm",
                    ["Arm 3"]     = "left-leg",
                    ["Thruster"]  = "right-leg"
                };
                var drs = new Dictionary<string, (int phys, int en, int rad)>();
                foreach (var armor in RobotArmors ?? [])
                {
                    var loc = ExtractParenthetical(armor.Name);
                    if (loc != null && robotLocMap.TryGetValue(loc, out var prefix))
                    {
                        var (p, e, r) = drs.GetValueOrDefault(prefix);
                        drs[prefix] = (p + armor.ResistancePhysical, e + armor.ResistanceEnergy, r);
                    }
                }
                WriteLocationDRs(fields, drs);
            }
            else
            {
                var humanLocMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["Head"]      = "head",
                    ["Torso"]     = "torso",
                    ["Left Arm"]  = "left-arm",
                    ["Right Arm"] = "right-arm",
                    ["Left Leg"]  = "left-leg",
                    ["Right Leg"] = "right-leg"
                };
                var drs = new Dictionary<string, (int phys, int en, int rad)>();
                foreach (var armor in Armors ?? [])
                    foreach (var loc in armor.Locations ?? [])
                        if (humanLocMap.TryGetValue(loc, out var prefix))
                        {
                            var (p, e, r) = drs.GetValueOrDefault(prefix);
                            drs[prefix] = (p + armor.ResistancePhysical, e + armor.ResistanceEnergy, r + armor.ResistanceRadiation);
                        }
                WriteLocationDRs(fields, drs);
            }

            // ── Weapons (up to 5) ─────────────────────────────────────────────────
            var weapons = Weapons ?? [];
            for (int i = 0; i < Math.Min(weapons.Count, 5); i++)
            {
                int n = i + 1;
                var w = weapons[i];
                fields[$"weapons_name{n}"]      = w.Name;
                fields[$"weapons_skill{n}"]      = w.WeaponType;
                fields[$"weapons_damage{n}"]     = w.Damage.ToString();
                fields[$"weapons_effects{n}"]    = string.Join(", ", w.Effects ?? []);
                fields[$"weapons_type{n}"]       = w.DamageType;
                fields[$"weapons_rate{n}"]       = w.FireRate > 0 ? w.FireRate.ToString() : string.Empty;
                fields[$"weapons_range{n}"]      = w.Range ?? string.Empty;
                fields[$"weapons_qualities{n}"]  = string.Join(", ", w.Qualities ?? []);
                fields[$"weapons_ammo{n}"]       = w.Ammunition ?? string.Empty;
                fields[$"weapons_weight{n}"]     = w.Weight.ToString();
            }

            // ── Ammo (up to 8) ────────────────────────────────────────────────────
            var ammos = Ammos ?? [];
            for (int i = 0; i < Math.Min(ammos.Count, 8); i++)
            {
                int n = i + 1;
                fields[$"ammo_caliber{n}"]         = ammos[i].Name;
                fields[$"ammo_caliber_quantity{n}"] = ammos[i].Quantity?.ToString() ?? string.Empty;
            }

            // ── Gear: clothing + consumeables + items (up to 18) ──────────────────
            var gear = new List<(string name, string lbs)>();
            foreach (var c in Clothings ?? [])
                gear.Add((c.Name, c.Weight > 0 ? c.Weight.ToString() : string.Empty));
            foreach (var c in Consumeables ?? [])
            {
                var name = (c.Quantity ?? 0) > 1 ? $"{c.Name} x{c.Quantity}" : c.Name;
                gear.Add((name, c.Weight > 0 ? c.Weight.ToString() : string.Empty));
            }
            foreach (var item in Items ?? [])
            {
                var name = (item.Quantity ?? 0) > 1 ? $"{item.Name} x{item.Quantity}" : item.Name;
                gear.Add((name, item.Weight > 0 ? item.Weight.ToString() : string.Empty));
            }
            for (int i = 0; i < Math.Min(gear.Count, 18); i++)
            {
                int n = i + 1;
                fields[$"gear_item{n}"]     = gear[i].name;
                fields[$"gear_item_lbs{n}"] = gear[i].lbs;
            }
            fields["current_carry_weight"] = string.Empty;
            fields["maximum_carry_weight"] = CarryWeight.ToString();

            // ── Caps ──────────────────────────────────────────────────────────────
            fields["Caps"] = Caps.ToString();

            // ── Perks & Traits: traits first, then perks (up to 13) ───────────────
            var perksTraits = new List<(string name, string rank)>();
            foreach (var t in Traits ?? [])
                perksTraits.Add((t.Name, string.Empty));
            foreach (var p in Perks ?? [])
                perksTraits.Add((p.Name, p.Rank > 0 ? p.Rank.ToString() : string.Empty));
            for (int i = 0; i < Math.Min(perksTraits.Count, 13); i++)
            {
                int n = i + 1;
                fields[$"Perks_Traits_name{n}"]   = perksTraits[i].name;
                fields[$"Perks_Traits_rank{n}"]   = perksTraits[i].rank;
                fields[$"Perks_Traits_effect{n}"] = string.Empty;
            }

            return PDFSchema.Generate(fields, pdfPath);
        }

        // Extracts the text inside the trailing parentheses of a name, e.g.
        // "Factory Armor (Main Body)" → "Main Body"
        private static string? ExtractParenthetical(string name)
        {
            var m = Regex.Match(name, @"\(([^)]+)\)$");
            return m.Success ? m.Groups[1].Value.Trim() : null;
        }

        // Writes phys/en/rad DR values into the fields dict for each location prefix.
        // "toros_en_dr" is a typo baked into the PDF for the torso energy DR field.
        private static void WriteLocationDRs(
            Dictionary<string, string> fields,
            Dictionary<string, (int phys, int en, int rad)> drs)
        {
            foreach (var (prefix, dr) in drs)
            {
                fields[$"{prefix}_phys_dr"] = dr.phys > 0 ? dr.phys.ToString() : string.Empty;
                fields[$"{prefix}_rad_dr"]  = dr.rad > 0  ? dr.rad.ToString()  : string.Empty;
                // torso energy DR field has a typo in the PDF
                var enKey = prefix == "torso" ? "toros_en_dr" : $"{prefix}_en_dr";
                fields[enKey] = dr.en > 0 ? dr.en.ToString() : string.Empty;
            }
        }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment() {
                Id = Id,
                Name = Name,
                Image = Image,
                Level = Level.ToString(),
                LevelName = "Level",
                Details = $"{Origin.Name}",
                CharacterSheet = CharacterSheet
            };
        }
    }
}
