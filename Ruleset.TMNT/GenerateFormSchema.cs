using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Utility;

namespace TMNT
{
    public static class GenerateFormSchema
    {
        private static readonly List<object> _fields = [];
        private static readonly string _jsonFilesPath = $"{new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.Parent}/Ruleset.TMNT/Json/";
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = JsonContractResolver.Get(),
            Formatting = Formatting.None
        };

        public static void InitializeSchema()
        {
            try
            {
                var animals = Load<Animal>("Animals.json");
                var alignments = Load<Alignment>("Alignments.json");
                var educations = Load<Education>("Educations.json");
                var mutations = Load<Mutation>("Mutations.json");
                var organizations = Load<Organization>("Organizations.json");
                var attributes = Load<Attribute>("Attributes.json");
                var bipeds = Load<Biped>("Bipeds.json");
                var hands = Load<Hand>("Hands.json");
                var looks = Load<Look>("Looks.json");
                var naturalWeapons = Load<NaturalWeapon>("NaturalWeapons.json");
                var psionics = Load<Psionic>("Psionics.json");
                var speeches = Load<Speech>("Speechs.json");
                var skills = Load<Skill>("Skills.json");
                var weapons = Load<Weapon>("Weapons.json");
                var equipment = Load<Equipment>("Equipment.json");
                var vehicles = Load<Vehicle>("Vehicles.json");
                var armors = Load<Armor>("Armors.json");

                GenerateDescriptionSchema();
                GenerateAnimalSchema(animals);
                GenerateAlignmentSchema(alignments);
                GenerateEducationSchema(educations);
                GenerateMutationSchema(mutations);
                GenerateOrganizationSchema(organizations);
                GenerateAttributesSchema(attributes);
                GenerateAnimalDependentSchema(bipeds.Select(b => (b.Name, b.Description, b.Animal, b.Cost)).ToList(), "biped", "Biped");
                GenerateAnimalDependentSchema(hands.Select(h => (h.Name, h.Description, h.Animal, h.Cost)).ToList(), "hand", "Hand");
                GenerateAnimalDependentSchema(looks.Select(l => (l.Name, l.Description, l.Animal, l.Cost)).ToList(), "look", "Look");
                GenerateAnimalDependentSchema(naturalWeapons.Select(n => (n.Name, n.Description, n.Animal, n.Cost)).ToList(), "naturalWeapon", "Natural Weapon");
                GenerateAnimalDependentSchema(speeches.Select(s => (s.Name, s.Description, s.Animal, s.Cost)).ToList(), "speech", "Speech");
                GeneratePsionicSchema(psionics);
                GenerateSkillsSchema(skills);
                GenerateWeaponsSchema(weapons);
                GenerateEquipmentSchema(equipment);
                GenerateVehicleSchema(vehicles);
                GenerateArmorSchema(armors);

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

        private static void GenerateDescriptionSchema()
        {
            _fields.Add(new { name = "id", id = "id", label = "Id", type = "hidden", className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "name", id = "name", label = "Name", type = "text", className = "form-control", @default = "Unknown", tab = "Identity" });
            _fields.Add(new { name = "image", id = "image", label = "Image", type = "image", className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "origin", id = "origin", label = "Origin", type = "text", className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "disposition", id = "disposition", label = "Disposition", type = "text", className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "gender", id = "gender", label = "Gender", type = "text", className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "age", id = "age", label = "Age", type = "number", className = "form-control", @default = 0, tab = "Identity" });
            _fields.Add(new { name = "weight", id = "weight", label = "Weight", type = "text", className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "height", id = "height", label = "Height", type = "text", className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "size", id = "size", label = "Size", type = "text", className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "level", id = "level", label = "Level", type = "number", className = "form-control", validation = new { required = true, min = 1, max = 15 }, @default = 1, tab = "Origins" });
            _fields.Add(new { name = "xp", id = "xp", label = "XP", type = "number", className = "form-control", @default = 0, tab = "Stats" });
            _fields.Add(new { name = "hitPoints", id = "hitPoints", label = "Hit Points", type = "number", className = "form-control", @default = 0, tab = "Stats" });
            _fields.Add(new { name = "sdc", id = "sdc", label = "SDC", type = "number", className = "form-control", @default = 0, tab = "Stats" });
            _fields.Add(new { name = "notes", id = "notes", label = "Notes", type = "textarea", className = "form-control", tab = "Identity" });
        }

        private static void GenerateAnimalSchema(List<Animal> animals)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "animal";
            obj.label = "Animal";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var animal in animals)
            {
                obj.options.Add(new
                {
                    value = animal.Name,
                    label = animal.Name,
                    description = animal.Description,
                    size = animal.Size,
                    length = animal.Length,
                    weight = animal.Weight,
                    build = animal.Build,
                    bio = animal.Bio,
                    bonusAdjustments = JsonConvert.SerializeObject(animal.BonusAdjustments, _jsonSettings)
                });
            }

            obj.tab = "Origins";
            _fields.Add(obj);
        }

        private static void GenerateAlignmentSchema(List<Alignment> alignments)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "alignment";
            obj.label = "Alignment";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var alignment in alignments)
            {
                obj.options.Add(new
                {
                    value = alignment.Name,
                    label = alignment.Name,
                    description = alignment.Description,
                    type = alignment.Type
                });
            }

            obj.tab = "Origins";
            _fields.Add(obj);
        }

        private static void GenerateEducationSchema(List<Education> educations)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "education";
            obj.label = "Education";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var education in educations)
            {
                obj.options.Add(new
                {
                    value = education.Name,
                    label = education.Name,
                    description = education.Description,
                    bonusAdjustments = JsonConvert.SerializeObject(education.BonusAdjustments, _jsonSettings)
                });
            }

            obj.tab = "Origins";
            _fields.Add(obj);
        }

        private static void GenerateMutationSchema(List<Mutation> mutations)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "mutation";
            obj.label = "Mutation";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var mutation in mutations)
            {
                obj.options.Add(new
                {
                    value = mutation.Name,
                    label = mutation.Name,
                    description = mutation.Description
                });
            }

            obj.tab = "Origins";
            _fields.Add(obj);
        }

        private static void GenerateOrganizationSchema(List<Organization> organizations)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "organization";
            obj.label = "Organization";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var org in organizations)
            {
                obj.options.Add(new
                {
                    value = org.Name,
                    label = org.Name,
                    description = org.Description
                });
            }

            obj.tab = "Origins";
            _fields.Add(obj);
        }

        private static void GenerateAttributesSchema(List<Attribute> attributes)
        {
            var children = new List<object>();

            foreach (var attr in attributes)
            {
                children.Add(new
                {
                    name = $"attributes.{attr.Name}",
                    id = $"attributes.{attr.Name.ToLower()}",
                    label = attr.Name,
                    description = attr.Description,
                    fullName = attr.FullName,
                    type = "number",
                    className = "form-control",
                    validation = new { required = true, min = 1, max = 30 },
                    @default = 0
                });
            }

            _fields.Add(new { type = "group", name = "attributes", label = "Attributes", children, tab = "Attributes" });
        }

        private static void GenerateAnimalDependentSchema(List<(string Name, string Description, string Animal, int Cost)> items, string fieldName, string fieldLabel)
        {
            var animalGroups = items.GroupBy(i => i.Animal);

            foreach (var group in animalGroups)
            {
                dynamic obj = new ExpandoObject();
                obj.name = $"{fieldName}.{group.Key.ToLower()}";
                obj.label = fieldLabel;
                obj.type = "select";
                obj.className = "form-select";
                obj.options = new List<object>();
                obj.dependsOn = new { field = "animal", value = group.Key };

                foreach (var (name, description, _, cost) in group)
                {
                    obj.options.Add(new
                    {
                        value = name,
                        label = name,
                        description,
                        cost
                    });
                }

                obj.tab = "Features";
                _fields.Add(obj);
            }
        }

        private static void GeneratePsionicSchema(List<Psionic> psionics)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "psionic";
            obj.label = "Psionics";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var psionic in psionics)
            {
                obj.options.Add(new
                {
                    value = psionic.Name,
                    label = psionic.Name,
                    description = psionic.Description,
                    range = psionic.Range,
                    duration = psionic.Duration,
                    savingThrow = psionic.SavingThrow
                });
            }

            dynamic array = new
            {
                name = "psionics",
                label = "Psionics",
                type = "array",
                component = obj,
                tab = "Features"
            };

            _fields.Add(array);
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
                    @default = 0,                    
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

        private static void GenerateWeaponsSchema(List<Weapon> weapons)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "weapons";
            obj.label = "Weapons";
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
                    twoHanded = weapon.TwoHanded,
                    length = weapon.Length,
                    weight = weapon.Weight,
                    damage = weapon.Damage,
                    cost = weapon.Cost,
                    cartridge = weapon.Cartridge,
                    feed = weapon.Feed,
                    range = weapon.Range
                });
            }

            dynamic array = new
            {
                name = "weapons",
                label = "Weapons",
                type = "array",
                component = obj,
                tab = "Equipment"
            };

            _fields.Add(array);
        }

        private static void GenerateEquipmentSchema(List<Equipment> equipment)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "equipments";
            obj.label = "Equipment";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var item in equipment)
            {
                obj.options.Add(new
                {
                    value = item.Name,
                    label = item.Name,
                    description = item.Description,
                    cost = item.Cost,
                    type = item.Type
                });
            }

            dynamic array = new
            {
                name = "equipments",
                label = "Equipment",
                type = "array",
                component = obj,
                tab = "Equipment"
            };

            _fields.Add(array);
        }

        private static void GenerateVehicleSchema(List<Vehicle> vehicles)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "vehicle";
            obj.label = "Vehicle";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var vehicle in vehicles)
            {
                obj.options.Add(new
                {
                    value = vehicle.Name,
                    label = vehicle.Name,
                    description = vehicle.Description,
                    cost = vehicle.Cost
                });
            }

            obj.tab = "Equipment";
            _fields.Add(obj);
        }

        private static void GenerateArmorSchema(List<Armor> armors)
        {
            dynamic obj = new ExpandoObject();
            obj.name = "armor";
            obj.label = "Armor";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var armor in armors)
            {
                obj.options.Add(new
                {
                    value = armor.Name,
                    label = armor.Name,
                    description = armor.Description,
                    ar = armor.AR,
                    sdc = armor.SDC,
                    weight = armor.Weight,
                    cost = armor.Cost
                });
            }

            obj.tab = "Equipment";
            _fields.Add(obj);
        }
    }
}