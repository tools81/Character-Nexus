using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ghostbusters
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = $"{new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.Parent}/Ruleset.Ghostbusters/Json/";
        private static readonly Regex sWhitespace = new Regex(@"\s+");

        public static void InitializeSchema()
        {
            try
            {
                string jsonGoalsData = File.ReadAllText(_jsonFilesPath + "Goals.json");
                string jsonTalentsData = File.ReadAllText(_jsonFilesPath + "Talents.json");
                string jsonTraitsData = File.ReadAllText(_jsonFilesPath + "Traits.json");                
                string jsonGearData = File.ReadAllText(_jsonFilesPath + "Gears.json");
                string jsonWeaponsData = File.ReadAllText(_jsonFilesPath + "Weapons.json");

                List<Talent>? talents = JsonConvert.DeserializeObject<List<Talent>?>(jsonTalentsData);

                if (talents == null)
                {
                    Console.WriteLine($"Unable to read talents json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Trait>? traits = JsonConvert.DeserializeObject<List<Trait>?>(jsonTraitsData);

                if (traits == null)
                {
                    Console.WriteLine($"Unable to read traits json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Goal>? goals = JsonConvert.DeserializeObject<List<Goal>?>(jsonGoalsData);

                if (goals == null)
                {
                    Console.WriteLine($"Unable to read goals json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Gear>? gears = JsonConvert.DeserializeObject<List<Gear>?>(jsonGearData);

                if (gears == null)
                {
                    Console.WriteLine($"Unable to read gear json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Weapon>? weapons = JsonConvert.DeserializeObject<List<Weapon>?>(jsonWeaponsData);

                if (weapons == null)
                {
                    Console.WriteLine($"Unable to read weapons json file. Aborting...");
                    Console.Read();
                    return;
                }

                GenerateDescriptionSchema();

                GenerateGoalSchema(goals, "goal", "Goal");
                GenerateTraitSchema(traits, "traits", "Trait Scores");
                GenerateTalentSchema(traits, talents, "talents", "Talents");                
                GenerateGearSchema(gears, "gears", "Gears");
                GenerateWeaponSchema(weapons, "weapons", "Weapons");

                var schema = new
                {
                    title = "Hero Editor",
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

        private static void GenerateDescriptionSchema()
        {
            _fields.Add(
                new
                {
                    name = "id",
                    id = "id",
                    label = "Id",
                    type = "hidden",
                    className = "form-control",
                    tab = "Identity"
                });
            _fields.Add(
                new
                {
                    name = "name",
                    id = "name",
                    label = "Name",
                    type = "text",
                    className = "form-control",
                    @default = "Unknown",
                    tab = "Identity"
                });
            _fields.Add(
                new
                {
                    name = "alias",
                    id = "alias",
                    label = "Alias",
                    type = "text",
                    className = "form-control",
                    @default = "Unknown",
                    tab = "Identity"
                });
            _fields.Add(
                new
                {
                    name = "browniePoints",
                    id = "browniePoints",
                    label = "Brownie Points",
                    type = "number",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 0,
                        max = 100
                    },
                    @default = 20,
                    tab = "Identity"
                });
            _fields.Add(
                new
                {
                    name = "image",
                    id = "image",
                    label = "Image",
                    type = "image",
                    className = "form-control",
                    tab = "Identity"
                });
            _fields.Add(
                new
                {
                    name = "description",
                    id = "description",
                    label = "Description",
                    type = "textarea",
                    className = "form-control",
                    tab = "Identity"
                });
            _fields.Add(
                new
                {
                    name = "notes",
                    id = "notes",
                    label = "Notes",
                    type = "textarea",
                    className = "form-control",
                    tab = "Identity"
                });
            _fields.Add(
                new
                {
                    name = "residence",
                    id = "residence",
                    label = "Residence",
                    type = "text",
                    className = "form-control",
                    tab = "Identity"
                });
            _fields.Add(
                new
                {
                    name = "telex",
                    id = "telex",
                    label = "Telex",
                    type = "text",
                    className = "form-control",
                    tab = "Identity"
                });
            _fields.Add(
                new
                {
                    name = "phone",
                    id = "phone",
                    label = "Phone",
                    type = "text",
                    className = "form-control",
                    tab = "Identity"
                });
        }

        private static void GenerateTraitSchema(List<Trait> traits, string name, string label)
        {
            var children = new List<object>();

            foreach (var trait in traits)
            {
                children.Add(new
                {
                    name = $"traits.{trait.Name}",
                    id = $"traits.{trait.Name.ToLower()}",
                    label = trait.Name,
                    type = "number",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 1,
                        max = 5
                    },
                    @default = 1
                });
            }

            var group = new
            {
                type = "group",
                name,
                label,
                children,
                tab = "Traits"
            };

            _fields.Add(group);
        }

        private static void GenerateTalentSchema(List<Trait> traits, List<Talent> talents, string name, string label)
        {
            foreach (var trait in traits)
            {
                _fields.Add( new
                {
                    name = $"{trait.Name}.label",
                    text = $"{trait.Name} Talent",
                    type = "textblock",
                    tab = "Talents"
                });

                dynamic obj = new ExpandoObject();

                obj.name = $"talents.{trait.Name}";
                obj.label = string.Empty;
                obj.type = "select";
                obj.className = "form-select";
                obj.options = new List<object>();

                foreach (var talent in talents.Where(t => t.Trait == trait.Name))
                {
                    obj.options.Add(
                        new
                        {
                            value = talent.Name,
                            label = talent.Name
                        }
                    );
                }

                obj.tab = "Talents";
                _fields.Add(obj);
            }
        }

        private static void GenerateGoalSchema(List<Goal> goals, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var goal in goals)
            {
                obj.options.Add(
                    new
                    {
                        value = goal.Name,
                        label = goal.Name,
                        description = goal.Description
                    }
                );
            }

            obj.tab = "Goal";
            _fields.Add(obj);
        }

        private static void GenerateGearSchema(List<Gear> gears, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var gear in gears)
            {
                obj.options.Add(
                    new
                    {
                        value = gear.Name,
                        label = gear.Name
                    }
                );
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj,
                tab = "Equipment"
            };

            _fields.Add(array);
        }

        private static void GenerateWeaponSchema(List<Weapon> weapons, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var weapon in weapons)
            {
                obj.options.Add(
                    new
                    {
                        value = weapon.Name,
                        label = weapon.Name
                    }
                );
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj,
                tab = "Equipment"
            };

            _fields.Add(array);
        }
    }
}