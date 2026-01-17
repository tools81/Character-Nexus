using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                string jsonTalentsData = File.ReadAllText(_jsonFilesPath + "Talents.json");
                string jsonTraitsData = File.ReadAllText(_jsonFilesPath + "Traits.json");
                string jsonGoalsData = File.ReadAllText(_jsonFilesPath + "Goals.json");
                string jsonEquipmentData = File.ReadAllText(_jsonFilesPath + "Equipment.json");

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

                List<Equipment>? equipments = JsonConvert.DeserializeObject<List<Equipment>?>(jsonEquipmentData);

                if (equipments == null)
                {
                    Console.WriteLine($"Unable to read equipment json file. Aborting...");
                    Console.Read();
                    return;
                }
                GenerateDescriptionSchema();

                GenerateTraitSchema(traits, "traits", "Trait Scores");
                GenerateTalentSchema(traits, talents, "talents", "Talents");
                GenerateGoalSchema(goals, "goal", "Goal");
                GenerateEquipmentSchema(equipments, "equipments", "Equipment");

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
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "name",
                    id = "name",
                    label = "Name",
                    type = "text",
                    className = "form-control",
                    @default = "Unknown"
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
                    @default = 20
                });
            _fields.Add(
                new
                {
                    name = "image",
                    id = "image",
                    label = "Image",
                    type = "image",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "personality",
                    id = "personality",
                    label = "Personality",
                    type = "textarea",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "notes",
                    id = "notes",
                    label = "Notes",
                    type = "textarea",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "residence",
                    id = "residence",
                    label = "Residence",
                    type = "text",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "telex",
                    id = "telex",
                    label = "Telex",
                    type = "text",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "phone",
                    id = "phone",
                    label = "Phone",
                    type = "text",
                    className = "form-control"
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
                        min = 0,
                        max = 5
                    },
                    @default = 0
                });
            }

            var group = new
            {
                type = "group",
                name,
                label,
                children
            };

            _fields.Add(group);
        }

        private static void GenerateTalentSchema(List<Trait> traits, List<Talent> talents, string name, string label)
        {
            foreach (var trait in traits)
            {
                dynamic obj = new ExpandoObject();

                obj.name = name;
                obj.label = label;
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
                        label = goal.Name
                    }
                );
            }
            _fields.Add(obj);

            foreach (var goal in goals)
            {
                var children = new List<object>
                {
                    new
                    {
                        name = $"info-{name}-{goal.Name}",
                        label = "Information",
                        type = "textblock",
                        className = "text-block",
                        text = goal.Description
                    }
                };

                var div = new
                {
                    type = "div",
                    className = "alert alert-secondary",
                    children,
                    dependsOn =
                        new
                        {
                            field = name,
                            value = goal.Name
                        }
                };

                _fields.Add(div);
            }
        }

        private static void GenerateEquipmentSchema(List<Equipment> equipments, string name, string label)
        {
            //TODO: Get the image and description in the item

            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var equipment in equipments)
            {
                obj.options.Add(
                    new
                    {
                        value = equipment.Name,
                        label = equipment.Name
                    }
                );
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj
            };

            _fields.Add(array);
        }
    }
}
