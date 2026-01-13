using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Utility;

namespace Transformers
{
    public class Ruleset : IRuleset
    {
        public string Name => "Transformers";
        public string RulesetName => "Ruleset.Transformers";
        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_transformers.jpg";
        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_transformers.png";
        public string FormResource => "Ruleset.Transformers.Json.Character.Form.json";
        public string  Instructions => string.Empty;

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