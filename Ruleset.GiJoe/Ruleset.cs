using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiJoe
{
    public class Ruleset : IRuleset
    {
        public string Name => "G.I. Joe";

        public string RulesetName => "Ruleset.GiJoe";

        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_gi_joe.jpg";

        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_gi_joe.png";

        public string NewCharacter()
        {
            throw new NotImplementedException();
        }
    }
}
