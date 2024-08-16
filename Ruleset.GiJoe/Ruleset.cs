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

        public string LoadCharacter(ICharacter character)
        {
            throw new NotImplementedException();
        }
    }
}
