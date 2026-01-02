using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Utility;

namespace AmazingTales
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
                string jsonGendersData = File.ReadAllText(_jsonFilesPath + "Genders.json");
                string jsonClansData = File.ReadAllText(_jsonFilesPath + "Clans.json");
                string jsonSkillsData = File.ReadAllText(_jsonFilesPath + "Skills.json");

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

                List<Skill>? skills = JsonConvert.DeserializeObject<List<Skill>?>(jsonSkillsData);

                if (skills == null)
                {
                    Console.WriteLine($"Unable to read skills json file. Aborting...");
                    Console.Read();
                    return;
                }

                GenerateDescriptionSchema();

                GenerateGenderSchema(genders, "gender", "Gender");
                GenerateClanSchema(clans, "clan", "Clan");
                GenerateSkillsSchema(skills, "skills", "Skills");

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
                        bonusCharacteristics = JsonConvert.SerializeObject(element.BonusCharacteristics, _jsonSettings)
                    }
                );
            }

            _fields.Add(obj);

            //Add a div below dropdown after selecting a value, containing details of Origin
            foreach (var element in elements)
            {
                var children = new List<object>
                {
                    new
                    {
                        name = $"info-{name}-{element.Name}",
                        label = "Information",
                        type = "textblock",
                        className = "text-block",
                        text = element.Description
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
                            value = element.Name
                        }
                };

                _fields.Add(div);
            }
        }

        public static void GenerateSkillsSchema(List<Skill> elements, string name, string label)
        {
            var children = new List<object>();

            foreach (var attribute in attributes)
            {
                children.Add(new
                {
                    name = $"attributes.{attribute.Name}",
                    id = $"attributes.{attribute.Name.ToLower()}",
                    label = attribute.Name,
                    type = "number",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = -3,
                        max = 9
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
    }
}
