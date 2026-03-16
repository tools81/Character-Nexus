using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Utility;

namespace WorldWideWrestling
{
    public class Ruleset : IRuleset
    {
        public string Name => "World Wide Wrestling";
        public string RulesetName => "Ruleset.WorldWideWrestling";
        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_world_wide_wrestling.jpg";
        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_world_wide_wrestling.jpg";
        public string FormResource => "Ruleset.WorldWideWrestling.Json.Character.Form.json";
        public string Instructions => File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/World_Wide_Wrestling_Instructions.html");
        public string Stylesheet => File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/World_Wide_Wrestling.css");

        public string LoadCharacter(ICharacter character)
        {
            return JsonConvert.SerializeObject(character, new CharacterJsonConverter());
        }

        public string NewCharacter()
        {
            string jsonObject;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(FormResource))
            {
                using (var reader = new StreamReader(stream))
                {
                    jsonObject = reader.ReadToEnd();
                }
            }

            return jsonObject;
        }

        public ICharacter SaveCharacter(string data)
        {
            try
            {
                var character = JsonConvert.DeserializeObject<Character>(data, new CharacterJsonConverter());
                return character;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing Json on save character: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving character: {ex.Message}");
                return null;
            }
        }
    }
}