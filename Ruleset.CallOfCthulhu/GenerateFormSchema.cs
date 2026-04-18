using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Utility;

namespace CallOfCthulhu
{
    public static class GenerateFormSchema
    {
        private static readonly List<object> _fields = [];
        private static readonly string _jsonFilesPath = $"{new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.Parent}/Ruleset.CallOfCthulhu/Json/";
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = JsonContractResolver.Get(),
            Formatting = Formatting.None
        };

        public static void InitializeSchema()
        {
            try
            {
                var eras = Load<Era>("Eras.json");
                var ages = Load<Age>("Ages.json");
                var occupations = Load<Occupation>("Occupations.json");
                var characteristics = Load<Characteristic>("Characteristics.json");
                var skills = Load<Skill>("Skills.json");
                var phobias = Load<Phobia>("Phobias.json");
                var manias = Load<Mania>("Manias.json");
                var spells = Load<Spell>("Spells.json");
                var weapons = Load<Weapon>("Weapons.json");
                var equipments = Load<Equipment>("Equipments.json");

                GenerateDescriptionSchema(eras, ages);
                GenerateOccupationSchema(occupations);
                GenerateCharacteristicsSchema(characteristics);
                GenerateDerivedStatsSchema();
                GenerateBiographySchema();
                GenerateSkillsSchema(skills);
                GeneratePhobiasSchema(phobias);
                GenerateManiaSchema(manias);
                GenerateSpellsSchema(spells);
                GenerateWeaponsSchema(weapons);
                GenerateEquipmentSchema(equipments);
                GenerateFinanceSchema();
                GenerateNotesSchema();

                var schema = new
                {
                    title = "Character Editor",
                    fields = _fields
                };

                string schemaJson = JsonConvert.SerializeObject(schema, Formatting.Indented);

                var schemaPath = _jsonFilesPath + "Character/Form.json";
                File.WriteAllText(schemaPath, schemaJson);

                Console.WriteLine("Character schema generated and saved to " + schemaPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static List<T> Load<T>(string fileName)
        {
            string json = File.ReadAllText(_jsonFilesPath + fileName);
            var result = JsonConvert.DeserializeObject<List<T>>(json);
            if (result == null) { Console.WriteLine($"Unable to read {fileName}. Aborting..."); Console.Read(); return []; }
            return result;
        }

        private static void GenerateDescriptionSchema(List<Era> eras, List<Age> ages)
        {
            _fields.Add(new { name = "id", id = "id", label = "Id", type = "hidden", className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "name", id = "name", label = "Name", type = "text", className = "form-control", @default = "Unknown", tab = "Identity" });
            _fields.Add(new { name = "image", id = "image", label = "Image", type = "image", className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "pronoun", id = "pronoun", label = "Pronoun", type = "text", className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "birthplace", id = "birthplace", label = "Birthplace", type = "text", className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "residence", id = "residence", label = "Residence", type = "text", className = "form-control", tab = "Identity" });

            // Era select
            dynamic eraObj = new ExpandoObject();
            eraObj.name = "era";
            eraObj.label = "Era";
            eraObj.type = "select";
            eraObj.className = "form-select";
            eraObj.options = new List<object>();
            foreach (var era in eras)
            {
                eraObj.options.Add(new { value = era.Name, label = era.Name, description = era.Description });
            }
            eraObj.tab = "Origins";
            _fields.Add(eraObj);

            // Age select
            dynamic ageObj = new ExpandoObject();
            ageObj.name = "age";
            ageObj.label = "Age";
            ageObj.type = "select";
            ageObj.className = "form-select";
            ageObj.options = new List<object>();
            foreach (var age in ages)
            {
                ageObj.options.Add(new
                {
                    value = age.Name,
                    label = age.Name,
                    description = age.Description,
                    bonusAdjustments = JsonConvert.SerializeObject(age.BonusAdjustments, _jsonSettings),
                    userChoices = JsonConvert.SerializeObject(age.UserChoices, _jsonSettings)
                });
            }
            ageObj.tab = "Origins";
            _fields.Add(ageObj);
        }

        private static void GenerateOccupationSchema(List<Occupation> occupations)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "occupation";
            obj.label = "Occupation";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var occ in occupations)
            {
                obj.options.Add(new
                {
                    value = occ.Name,
                    label = occ.Name,
                    description = occ.Description,
                    credit = occ.Credit,
                    suggestedContacts = occ.SuggestedContacts,
                    skillPoints = occ.SkillPoints,
                    skills = occ.Skills,
                    specialtyCount = occ.SpecialtyCount
                });
            }

            obj.tab = "Origins";
            _fields.Add(obj);
        }

        private static void GenerateCharacteristicsSchema(List<Characteristic> characteristics)
        {
            var children = new List<object>();

            foreach (var characteristic in characteristics)
            {
                children.Add(new
                {
                    name = $"characteristics.{characteristic.Name}",
                    id = $"characteristics.{characteristic.Name.ToLower()}",
                    label = characteristic.Name,
                    description = characteristic.Description,
                    roll = characteristic.Roll,
                    type = "number",
                    className = "form-control",
                    validation = new { required = true, min = 1, max = 100 },
                    @default = 0
                });
            }

            _fields.Add(new { type = "group", name = "characteristics", label = "Characteristics", children, tab = "Characteristics" });
        }

        private static void GenerateSkillsSchema(List<Skill> skills)
        {
            var children = new List<object>();

            foreach (var skill in skills)
            {
                children.Add(new
                {
                    name = $"skills.{skill.Name}",
                    id = $"skills.{skill.Name}",
                    label = skill.Name,
                    type = "number",
                    className = "form-control",
                    @default = skill.Value,                    
                });
            }

            _fields.Add(new
            {
                type = "group",
                name = "skills",
                label = "Skills",
                children,
                tab = "Skills"
            });
        }

        private static void GenerateDerivedStatsSchema()
        {
            var children = new List<object>
            {
                new { name = "damageBonus", id = "damageBonus", label = "Damage Bonus", type = "number", className = "form-control", @default = 0 },
                new { name = "build", id = "build", label = "Build", type = "number", className = "form-control", @default = 0 },
                new { name = "hitPoints", id = "hitPoints", label = "Hit Points", type = "number", className = "form-control", @default = 0 },
                new { name = "movement", id = "movement", label = "Movement", type = "number", className = "form-control", @default = 0 },
                new { name = "sanityPoints", id = "sanityPoints", label = "Sanity Points", type = "number", className = "form-control", @default = 0 },
                new { name = "magicPoints", id = "magicPoints", label = "Magic Points", type = "number", className = "form-control", @default = 0 }
            };

            _fields.Add(new { type = "group", name = "derivedStats", label = "Derived Stats", children, tab = "Characteristics" });
        }

        private static void GenerateBiographySchema()
        {
            _fields.Add(new { type = "divider", tab = "Background" });
            _fields.Add(new { type = "textblock", label = "Background", text = "Background", name = "backgroundLabel", tab = "Background" });

            _fields.Add(new { name = "description", id = "description", label = "Description", type = "textarea", className = "form-control", tab = "Background" });
            _fields.Add(new { name = "ideology", id = "ideology", label = "Ideology / Beliefs", type = "textarea", className = "form-control", tab = "Background" });
            _fields.Add(new { name = "significantPeople", id = "significantPeople", label = "Significant People", type = "textarea", className = "form-control", tab = "Background" });
            _fields.Add(new { name = "meaningfulLocations", id = "meaningfulLocations", label = "Meaningful Locations", type = "textarea", className = "form-control", tab = "Background" });
            _fields.Add(new { name = "treasuredPosessions", id = "treasuredPosessions", label = "Treasured Possessions", type = "textarea", className = "form-control", tab = "Background" });
            _fields.Add(new { name = "traits", id = "traits", label = "Traits", type = "textarea", className = "form-control", tab = "Background" });
            _fields.Add(new { name = "scars", id = "scars", label = "Injuries & Scars", type = "textarea", className = "form-control", tab = "Background" });
            _fields.Add(new { name = "encounters", id = "encounters", label = "Encounters with Strange Entities", type = "textarea", className = "form-control", tab = "Background" });
            _fields.Add(new { name = "story", id = "story", label = "Story", type = "textarea", className = "form-control", tab = "Background" });

            _fields.Add(new { type = "divider", tab = "Background" });
        }

        private static void GeneratePhobiasSchema(List<Phobia> phobias)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "phobia";
            obj.label = "Phobia";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var phobia in phobias)
            {
                obj.options.Add(new { value = phobia.Name, label = phobia.Name, description = phobia.Description });
            }

            _fields.Add(new { name = "phobias", label = "Phobias", type = "array", component = obj, tab = "Features" });
        }

        private static void GenerateManiaSchema(List<Mania> manias)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "mania";
            obj.label = "Mania";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var mania in manias)
            {
                obj.options.Add(new { value = mania.Name, label = mania.Name, description = mania.Description });
            }

            _fields.Add(new { name = "manias", label = "Manias", type = "array", component = obj, tab = "Features" });
        }

        private static void GenerateSpellsSchema(List<Spell> spells)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "spell";
            obj.label = "Spell";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var spell in spells)
            {
                obj.options.Add(new
                {
                    value = spell.Name,
                    label = spell.Name,
                    description = spell.Description,
                    cost = spell.Cost,
                    time = spell.Time
                });
            }

            _fields.Add(new { name = "spells", label = "Spells", type = "array", component = obj, tab = "Features" });
        }

        private static void GenerateWeaponsSchema(List<Weapon> weapons)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "weapon";
            obj.label = "Weapon";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var weapon in weapons)
            {
                obj.options.Add(new
                {
                    value = weapon.Name,
                    label = weapon.Name,
                    description = weapon.Description,
                    skill = weapon.Skill,
                    damage = weapon.Damage,
                    range = weapon.Range,
                    uses = weapon.Uses,
                    magazine = weapon.Magazine,
                    cost = weapon.Cost,
                    malfunction = weapon.Malfunction,
                    eras = weapon.Eras
                });
            }

            _fields.Add(new { name = "weapons", label = "Weapons", type = "array", component = obj, tab = "Equipment" });
        }

        private static void GenerateEquipmentSchema(List<Equipment> equipments)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "equipment";
            obj.label = "Equipment";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var item in equipments)
            {
                obj.options.Add(new
                {
                    value = item.Name,
                    label = item.Name,
                    description = item.Description,
                    cost = item.Cost,
                    era = item.Era
                });
            }

            _fields.Add(new { name = "equipments", label = "Equipment", type = "array", component = obj, tab = "Equipment" });
        }

        private static void GenerateFinanceSchema()
        {
            var children = new List<object>
            {
                new { name = "creditRating", id = "creditRating", label = "Credit Rating", type = "text", className = "form-control" },
                new { name = "spendingLevel", id = "spendingLevel", label = "Spending Level", type = "text", className = "form-control" },
                new { name = "cash", id = "cash", label = "Cash", type = "number", className = "form-control", @default = 0 },
                new { name = "assets", id = "assets", label = "Assets", type = "textarea", className = "form-control" }
            };

            _fields.Add(new { type = "group", name = "finance", label = "Finance", children, tab = "Finance" });
        }

        private static void GenerateNotesSchema()
        {
            _fields.Add(new { name = "notes", id = "notes", label = "Notes", type = "textarea", className = "form-control", tab = "Finance" });
        }
    }
}
