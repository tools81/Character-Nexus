using Utility;
using System;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;

namespace AmazingTales
{
    public class Ruleset : IRuleset
    {
        public string Name => "Amazing Tales";
        public string RulesetName => "Ruleset.AmazingTales";
        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_amazing_tales.jpg";
        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_amazing_tales.png";

        public string FormResource => "Ruleset.AmazingTales.Json.Character.Form.json";
        public string  Instructions => File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Instructions.html");

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

        public ICharacter? SaveCharacter(string data)
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

        public string LoadCharacter(ICharacter character)
        {
            return JsonConvert.SerializeObject(character, new CharacterJsonConverter());
        }

        public bool DeleteCharacter(string id)
        {
            throw new NotImplementedException();
        }
    }
}
