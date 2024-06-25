using Utility;

namespace Marvel
{
    public class Ruleset : IRuleset
    {
        public string Name => "Marvel Multiverse";

        public string RulesetName => "Ruleset.Marvel";

        public string ImageSource => "https://characternexus.file.core.windows.net/resources/card_marvel.jpg";

        public string LogoSource => "https://characternexus.file.core.windows.net/resources/logo_marvel.png";

        public string NewCharacter()
        {
            throw new NotImplementedException();
        }
    }
}