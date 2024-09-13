using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace EverydayHeroes
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = "C:/Users/toole/OneDrive/Source/Character Nexus/Ruleset.EverydayHeroes/Json/";

        public static void InitializeSchema()
        {
            try
            {
                string jsonArchetypesData = File.ReadAllText(_jsonFilesPath + "Archetypes.json");
                List<Archetype> archetypes = JsonConvert.DeserializeObject<List<Archetype>>(jsonArchetypesData);

                if (archetypes == null)
                {
                    Console.WriteLine($"Unable to read archetypes json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonClassesData = File.ReadAllText(_jsonFilesPath + "Classes.json");
                List<Class> classes = JsonConvert.DeserializeObject<List<Class>>(jsonClassesData);

                if (classes == null)
                {
                    Console.WriteLine($"Unable to read classes json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonBackgroundsData = File.ReadAllText(_jsonFilesPath + "Backgrounds.json");
                List<Background> backgrounds = JsonConvert.DeserializeObject<List<Background>>(jsonBackgroundsData);

                if (backgrounds == null)
                {
                    Console.WriteLine($"Unable to read backgrounds json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonProfessionsData = File.ReadAllText(_jsonFilesPath + "Professions.json");
                List<Profession> professions = JsonConvert.DeserializeObject<List<Profession>>(jsonProfessionsData);

                if (professions == null)
                {
                    Console.WriteLine($"Unable to read professions json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonAttributesData = File.ReadAllText(_jsonFilesPath + "Attributes.json");
                List<Attribute> attributes = JsonConvert.DeserializeObject<List<Attribute>>(jsonAttributesData);

                if (attributes == null)
                {
                    Console.WriteLine($"Unable to read attributes json file. Aborting...");
                    Console.Read();
                    return;
                }

                AddDescriptionFields();

                GenerateSchema(archetypes, "select");
                GenerateSchema(classes, archetypes, "select");
                GenerateSchema(backgrounds, "select");
                GenerateSchema(professions, "select");

                GenerateSchema
                (
                    attributes,
                    "number",
                    true,
                    new
                    {
                        required = true,
                        min = 0,
                        max = 30
                    }
                );

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

        private static void AddDescriptionFields()
        {
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
                    type = "file",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "level",
                    id = "level",
                    label = "Level",
                    type = "number",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 1,
                        max = 20
                    },
                    @default = 1
                });
        }

        public static void GenerateSchema(List<Archetype> archetypes, string fieldType)
        {
            dynamic obj = new ExpandoObject();

            obj.name = "archetype";
            obj.label = "Archetype";
            obj.type = fieldType;
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var archetype in archetypes)
            {
                obj.options.Add(
                    new
                    {
                        value = archetype.Name,
                        label = archetype.Name
                    }
                );
            }

            _fields.Add(obj);

            foreach (var archetype in archetypes)
            {
                var children = new List<object>
                {
                    new
                    {
                        name = $"info-archetype-{archetype.Name}",
                        label = "Information",
                        type = "textblock",
                        className = "text-block",
                        text = archetype.Description
                    }
                };

                var div = new
                {
                    type = "div",
                    className = "form-section",
                    children,
                    dependsOn =
                        new
                        {
                            field = "archetype",
                            value = archetype.Name
                        }
                };

                _fields.Add(div);
            }           
        }

        public static void GenerateSchema(List<Class> classes, List<Archetype> archetypes, string fieldType)
        {
            foreach (var archetype in archetypes)
            {
                dynamic obj = new ExpandoObject();

                obj.name = "class";
                obj.label = "Class";
                obj.type = fieldType;
                obj.className = "form-select";
                obj.options = new List<object>();

                foreach (var cl in classes.Where(c => c.Archetype == archetype.Name))
                {
                    obj.options.Add(
                        new
                        {
                            value = cl.Name,
                            label = cl.Name
                        }
                    );

                    obj.dependsOn =
                        new
                        {
                            field = "archetype",
                            value = archetype.Name
                        };
                }

                _fields.Add(obj);
            }
        }

        public static void GenerateSchema(List<Background> backgrounds, string fieldType)
        {
            dynamic obj = new ExpandoObject();

            obj.name = "background";
            obj.label = "Background";
            obj.type = fieldType;
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var background in backgrounds)
            {
                obj.options.Add(
                    new
                    {
                        value = background.Name,
                        label = background.Name
                    }
                );
            }

            _fields.Add(obj);
        }

        public static void GenerateSchema(List<Profession> professions, string fieldType)
        {
            dynamic obj = new ExpandoObject();

            obj.name = "profession";
            obj.label = "Profession";
            obj.type = fieldType;
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var profession in professions)
            {
                obj.options.Add(
                    new
                    {
                        value = profession.Name,
                        label = profession.Name
                    }
                );
            }

            _fields.Add(obj);
        }

        public static void GenerateSchema(List<Attribute> attributes, string fieldType, bool hasDefault, object validation = null)
        {
            var children = new List<object>();
            foreach (var attribute in attributes)
            {
                children.Add(
                    new
                    {
                        name = attribute.Name.ToLower(),
                        label = attribute.Name,
                        id = attribute.Name,
                        type = fieldType,
                        className = "form-control",
                        @default = 0
                    }
                );            
            }

            var div = new
            {
                type = "div",
                className = "input-group mb-3 text-center",
                children
            };

            _fields.Add(div);
        }
    }
}