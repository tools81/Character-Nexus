using Newtonsoft.Json;
using System.Reflection;
using Utility;

namespace FinalFantasy
{
    public class Ruleset : IRuleset
    {
        public string Name => "Final Fantasy XIV";

        public string RulesetName => "Ruleset.FinalFantasy";

        public string ImageSource => $"https://characternexus.blob.core.windows.net/resources/card_final_fantasy.jpg";

        public string LogoSource => $"https://characternexus.blob.core.windows.net/resources/logo_final_fantasy.png";

        public string FormResource => "Ruleset.FinalFantasy.Json.Character.Form.json";
        public string  Instructions => File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/FFXIV_Instructions.html");
        public string Stylesheet => File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/FFXIV.css");


        public string NewCharacter()
        {
            string jsonObject;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(FormResource))
            {
                if (stream == null)
                {
                    return string.Empty;
                }

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
