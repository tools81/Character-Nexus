using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfinder
{
    public class Ruleset : IRuleset
    {
        public string Name => "Starfinder";
        public string RulesetName => "Ruleset.Starfinder";
        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_starfinder.jpg";
        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_starfinder.png";

        public Ruleset()
        {
            
        }

        public string NewCharacter()
        {
            throw new NotImplementedException();
        }

        public ICharacter? SaveCharacter(string data)
        {
            throw new NotImplementedException();
        }
    }
}
