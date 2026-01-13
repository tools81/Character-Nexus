using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Utility;

namespace DarkCrystal
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = "C:/Users/toole/OneDrive/Source/Character Nexus/Ruleset.DarkCrystal/Json/";
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = JsonContractResolver.Get(),
            Formatting = Formatting.None
        };

        public static void InitializeSchema()
        {
            try
            {
                string jsonGendersData = File.ReadAllText(_jsonFilesPath + "Gender.json");
                string jsonClansData = File.ReadAllText(_jsonFilesPath + "Clans.json");
                string jsonTraitsData = File.ReadAllText(_jsonFilesPath + "Traits.json");
                string jsonSkillsData = File.ReadAllText(_jsonFilesPath + "Skills.json");
                string jsonSpecializationsData = File.ReadAllText(_jsonFilesPath + "Specializations.json");
                string jsonFlawsData = File.ReadAllText(_jsonFilesPath + "Flaws.json");
                string jsonGearsData = File.ReadAllText(_jsonFilesPath + "Gear.json");

                List<Gender>? genders = JsonConvert.DeserializeObject<List<Gender>?>(jsonGendersData);
                if (genders == null)
                {
                    Console.WriteLine($"Unable to read genders json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Clan>? clans = JsonConvert.DeserializeObject<List<Clan>?>(jsonClansData);
                if (clans == null)
                {
                    Console.WriteLine($"Unable to read clans json file. Aborting...");
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

                List<Skill>? skills = JsonConvert.DeserializeObject<List<Skill>?>(jsonSkillsData);
                if (skills == null)
                {
                    Console.WriteLine($"Unable to read skills json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Specialization>? specializations = JsonConvert.DeserializeObject<List<Specialization>?>(jsonSpecializationsData);
                if (specializations == null)
                {
                    Console.WriteLine($"Unable to read specializations json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Flaw>? flaws = JsonConvert.DeserializeObject<List<Flaw>?>(jsonFlawsData);
                if (flaws == null)
                {
                    Console.WriteLine($"Unable to read flaws json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Gear>? gears = JsonConvert.DeserializeObject<List<Gear>?>(jsonGearsData);
                if (gears == null)
                {
                    Console.WriteLine($"Unable to read gear json file. Aborting...");
                    Console.Read();
                    return;
                }

                GenerateDescriptionSchema();

                GenerateGenderSchema(genders, "gender", "Gender");
                GenerateClanSchema(clans, "clan", "Clan");
                GenerateTraitsSchema(traits, "trait", "Traits");
                GenerateFlawsSchema(flaws, "flaw", "Flaws");
                GenerateGearSchema(gears, "gear", "Gear");

                GenerateSkillsSchema(skills, "skill", "Skills", specializations, "specialization", "Specializations"); //Specializations are grouped under skills

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
                    name = "image",
                    id = "image",
                    label = "Image",
                    type = "image",
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
        }

        public static void GenerateGenderSchema(List<Gender> elements, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var element in elements)
            {
                obj.options.Add(
                    new
                    {
                        value = element.Name,
                        label = element.Name
                    }
                );
            }

            _fields.Add(obj);
        }

        public static void GenerateClanSchema(List<Clan> elements, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var element in elements)
            {
                obj.options.Add(
                    new
                    {
                        value = element.Name,
                        label = element.Name,
                        description = element.Description,
                        bonusCharacteristics = JsonConvert.SerializeObject(element.BonusCharacteristics, _jsonSettings),
                        userChoices = JsonConvert.SerializeObject(element.UserChoices, _jsonSettings)
                    }
                );
            }

            _fields.Add(obj);

            //Add a div below dropdown after selecting a value, containing details of Origin
            // foreach (var element in elements)
            // {
            //     var children = new List<object>
            //     {
            //         new
            //         {
            //             name = $"info-{name}-{element.Name}",
            //             label = "Information",
            //             image = element.image,
            //             type = "textblock",
            //             className = "text-block",
            //             text = element.Description
            //         }
            //     };

            //     var div = new
            //     {
            //         type = "div",
            //         className = "alert alert-secondary",
            //         children,
            //         dependsOn =
            //             new
            //             {
            //                 field = name,
            //                 value = element.Name
            //             }
            //     };

            //     _fields.Add(div);
            // }
        }

        public static void GenerateTraitsSchema(List<Trait> elements, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var element in elements)
            {
                obj.options.Add(
                    new
                    {
                        value = element.Name,
                        label = $"{element.Clan} - {element.Name}",
                        clan = element.Clan
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

        public static void GenerateSkillsSchema(List<Skill> elements, string name, string label, List<Specialization> specializations, string specName, string specLabel)
        {            
            foreach (var element in elements)
            {
                _fields.Add(new
                {
                    type = "divider"
                });

                var field = new
                {
                    name = $"skills.{element.Name.ToLower()}",
                    id = $"skills.{element.Name}",
                    label = element.Name,
                    type = "switch"
                };

                _fields.Add(field);

                dynamic obj = new ExpandoObject();

                obj.name = $"{specName}.{element.Name}";
                obj.id = $"{specName}.{element.Name}";
                obj.label = $"{element.Name} {specLabel}";
                obj.type = "select";
                obj.className = "form-select";
                obj.options = new List<object>();

                foreach (var specialization in specializations.Where(s => s.Skill.Contains(element.Name)))
                {
                    obj.options.Add(
                        new
                        {
                            value = specialization.Name,
                            label = specialization.Name,
                            description = specialization.Description
                        }
                    );
                }

                var arrayName = $"{element.Name}.{name}";

                dynamic array = new
                {
                    name = obj.name,
                    label = obj.label,
                    type = "array",
                    component = obj
                };

                _fields.Add(array);
            }
        }

        public static void GenerateFlawsSchema(List<Flaw> flaws, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var flaw in flaws)
            {
                obj.options.Add(
                    new
                    {
                        value = flaw.Name,
                        label = flaw.Name
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
                component = obj
            };

            _fields.Add(array);
        }
    }
}
