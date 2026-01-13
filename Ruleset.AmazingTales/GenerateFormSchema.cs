using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace AmazingTales
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = "C:/Users/toole/OneDrive/Source/Character Nexus/Ruleset.AmazingTales/Json/";
        private static readonly Regex sWhitespace = new Regex(@"\s+");

        public static void InitializeSchema()
        {
            try
            {
                GenerateDescriptionSchema();

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
                    type = "divider"
                });
            _fields.Add(
                new
                {
                    name = "d12Attribute",
                    id = "d12Attribute",
                    label = "D12 Skill",
                    type = "text",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "d10Attribute",
                    id = "d10Attribute",
                    label = "D10 Skill",
                    type = "text",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "d8Attribute",
                    id = "d8Attribute",
                    label = "D8 Skill",
                    type = "text",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "d6Attribute",
                    id = "d6Attribute",
                    label = "D6 Skill",
                    type = "text",
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
    }
}
