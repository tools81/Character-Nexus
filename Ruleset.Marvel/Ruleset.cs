using System.Reflection;
using Utility;

namespace Marvel
{
    public class Ruleset : IRuleset
    {
        public string Name => "Marvel Multiverse";

        public string RulesetName => "Ruleset.Marvel";

        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_marvel.jpg";

        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_marvel.png";

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
    }
}