using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Utility;

namespace BladeRunner
{
    public class Ruleset : IRuleset
    {
        public string Name => "Blade Runner";
        public string RulesetName => "Ruleset.BladeRunner";
        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_blade_runner.jpg";
        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_blade_runner.png";
        public string FormResource => "Ruleset.BladeRunner.Json.Character.Form.json";

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