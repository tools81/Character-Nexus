using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Utility;

namespace StarWars
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = $"{new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.Parent}/Ruleset.StarWars/Json/";
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
                var backgrounds = LoadJson.Load<Background>(_jsonFilesPath, "Backgroud.json");
                var obligations = LoadJson.Load<Obligation>(_jsonFilesPath, "Obligations.json");
                var ambitions = LoadJson.Load<Ambition>(_jsonFilesPath, "Ambitions.json");
                var causes = LoadJson.Load<Cause>(_jsonFilesPath, "Causes.json");
                var relationships = LoadJson.Load<Relationship>(_jsonFilesPath, "Relationships.json");
                var species = LoadJson.Load<Species>(_jsonFilesPath, "Species.json");
                var attributes = LoadJson.Load<Attribute>(_jsonFilesPath, "Attributes.json");
                var careers = LoadJson.Load<Career>(_jsonFilesPath, "Careers.json");
                var specializations = LoadJson.Load<Specialization>(_jsonFilesPath, "Specializations.json");
                var talents = LoadJson.Load<Talent>(_jsonFilesPath, "Talents.json");
                var talent_trees = LoadJson.Load<Talent_Tree>(_jsonFilesPath, "Talent_Trees.json");
                var skills = LoadJson.Load<Skill>(_jsonFilesPath, "Skills.json");
                var weapons = LoadJson.Load<Weapon>(_jsonFilesPath, "Weapons.json");
                var armors = LoadJson.Load<Armor>(_jsonFilesPath, "Armors.json");
                var gears = LoadJson.Load<Gear>(_jsonFilesPath, "Gears.json");
                var cybernetics = LoadJson.Load<Cybernetic>(_jsonFilesPath, "Cybernetics.json");
                var itemAttachments = LoadJson.Load<ItemAttachment>(_jsonFilesPath, "ItemAttachments.json");
                var itemMods = LoadJson.Load<ItemMod>(_jsonFilesPath, "ItemMods.json");
                var itemQualities = LoadJson.Load<ItemQuality>(_jsonFilesPath, "ItemQualities.json");

                GenerateDescriptionSchema();
                GenerateDerivedStatsSchema();

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
                Console.WriteLine(ex.ToString());
            }            
        }        

        private static void GenerateDescriptionSchema()
        {
            _fields.Add(new { name = "id", id = "id", label = "Id", type = "hidden", className = "form-control" });
            _fields.Add(new { name = "name", id = "name", label = "Name", type = "text", className = "form-control", @default = "Unknown" });
            _fields.Add(new { name = "image", id = "image", label = "Image", type = "image", className = "form-control" });            
            _fields.Add(new { name = "height", id = "height", label = "Height", type = "text", className = "form-control" });
            _fields.Add(new { name = "weight", id = "weight", label = "Weight", type = "text", className = "form-control" });
            _fields.Add(new { name = "build", id = "build", label = "Build", type = "text", className = "form-control" });
            _fields.Add(new { name = "hairColor", id = "hairColor", label = "Hair Color", type = "text", className = "form-control" });
            _fields.Add(new { name = "eyeColor", id = "eyeColor", label = "Eye Color", type = "text", className = "form-control" });
            _fields.Add(new { name = "skinColor", id = "skinColor", label = "Skin/Scale/Fur Color", type = "text", className = "form-control" });
            _fields.Add(new { name = "identifyingMarks", id = "identifyingMarks", label = "Identifying Marks", type = "textarea", className = "form-control" });
            _fields.Add(new { name = "personality", id = "personality", label = "Personality", type = "text", className = "form-control" });
            _fields.Add(new { name = "morality", id = "morality", label = "Morality", type = "number", className = "form-control", @default = 50 });
            _fields.Add(new { name = "notes", id = "notes", label = "Notes", type = "textarea", className = "form-control" });
            _fields.Add(new { name = "credits", id = "credits", label = "Credits", type = "number", className = "form-control", @default = 500 });
            _fields.Add(new { name = "experience", id = "experience", label = "Experience", type = "number", className = "form-control" });
        }

        private static void GenerateDerivedStatsSchema()
        {
            _fields.Add(
                new
                {
                    name = "woundThreshold",
                    id = "woundThreshold",
                    label = "Wound Threshold",
                    type = "number",
                    calculation = $"[attributes.Brawn]",
                    pinnedStat = true
                });
            _fields.Add(
                new
                {
                    name = "strainThreshold",
                    id = "strainThreshold",
                    label = "Strain Threshold",
                    type = "number",
                    calculation = $"[attributes.Willpower]",
                    pinnedStat = true
                });
            _fields.Add(
               new
               {
                   name = "meleeDefense",
                   id = "meleeDefense",
                   label = "Melee Defense",
                   type = "number",
                   pinnedStat = true
               });
            _fields.Add(
               new
               {
                   name = "rangedDefense",
                   id = "rangedDefense",
                   label = "Ranged Defense",
                   type = "number",
                   pinnedStat = true
               });
            _fields.Add(
                new
                {
                    name = "soak",
                    id = "soak",
                    label = "Soak",
                    type = "number",
                    calculation = $"[attributes.Brawn]",
                    pinnedStat = true
                });
        }
    }
}
