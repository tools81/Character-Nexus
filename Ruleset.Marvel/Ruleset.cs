using System.Reflection;
using Newtonsoft.Json;
using Utility;

namespace Marvel
{
    public class Ruleset : IRuleset
    {
        public string Name => "Marvel Multiverse";
        public string RulesetName => "Ruleset.Marvel";
        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_marvel.jpg";
        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_marvel.png";

        public Ruleset()
        {
            
        }

        public string NewCharacter()
        {
            string jsonObject;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Marvel.Json.Character.Form.json"))
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
                var character = JsonConvert.DeserializeObject<Character>(data);
                character?.SetBonusAdjustments(); 
                return character;
            }
            catch(JsonException ex)
            {
                Console.WriteLine($"Error parsing Json on save character: {ex.Message}");
                return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error saving character: {ex.Message}");
                return null;
            }
        }
    }
}