using Newtonsoft.Json;
using System.Reflection;
using Utility;

namespace HunterTheReckoning
{
    internal class Ruleset : IRuleset
    {
        public string Name => "Hunter The Reckoning";

        public string RulesetName => "Ruleset.HunterTheReckoning";

        public string ImageSource => throw new NotImplementedException();

        public string LogoSource => throw new NotImplementedException();

        public string FormResource => "Ruleset.HunterTheReckoning.Json.Character.Form.json";
        public string  Instructions => string.Empty;
        public string Stylesheet => string.Empty;

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
