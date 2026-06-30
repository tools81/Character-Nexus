using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Utility;

namespace HunterTheReckoning
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = $"{new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.Parent}/Ruleset.HunterTheReckoning/Json/";
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
                var creeds = LoadJson.Load<Creed>(_jsonFilesPath, "Creeds.json");
                var drives = LoadJson.Load<Drive>(_jsonFilesPath, "Drives.json");
                var skills = LoadJson.Load<Skill>(_jsonFilesPath, "Skills.json");
                var attributes = LoadJson.Load<Attribute>(_jsonFilesPath, "Attributes.json");
                var edges = LoadJson.Load<Edge>(_jsonFilesPath, "Edge.json");
                var perks = LoadJson.Load<Perk>(_jsonFilesPath, "Perks.json");
                var advantages = LoadJson.Load<Advantage>(_jsonFilesPath, "Advantages.json");
                var backgrounds = LoadJson.Load<Background>(_jsonFilesPath, "Backgrounds.json");
                var flaws = LoadJson.Load<Flaw>(_jsonFilesPath, "Flaws.json");
                var merits = LoadJson.Load<Merit>(_jsonFilesPath, "Merits.json");
                var weapons = LoadJson.Load<Weapon>(_jsonFilesPath, "Weapons.json");
                var armors = LoadJson.Load<Armor>(_jsonFilesPath, "Armors.json");
                var gears = LoadJson.Load<Gear>(_jsonFilesPath, "Gears.json");

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
                Console.WriteLine(ex.ToString());
            }
        }

        private static void GenerateDescriptionSchema()
        {
            _fields.Add(new { name = "id", id = "id", label = "Id", type = "hidden", className = "form-control" });
            _fields.Add(new { name = "name", id = "name", label = "Name", type = "text", className = "form-control", @default = "Unknown" });
            _fields.Add(new { name = "image", id = "image", label = "Image", type = "image", className = "form-control" });
            _fields.Add(new { name = "concept", id = "concept", label = "Concept", type = "textarea", className = "form-control" });
            _fields.Add(new { name = "ambition", id = "ambition", label = "Ambition", type = "textarea", className = "form-control" });
            _fields.Add(new { name = "desire", id = "desire", label = "Desire", type = "textarea", className = "form-control" });
            _fields.Add(new { name = "notes", id = "notes", label = "Notes", type = "textarea", className = "form-control" });
        }
    }
}
