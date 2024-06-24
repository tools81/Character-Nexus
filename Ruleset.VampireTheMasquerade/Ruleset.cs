using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampireTheMasquerade
{
    public class Ruleset : IRuleset
    {
        public string Name => "Vampire: The Masquerade";

        public string RulesetName => "Ruleset.VampireTheMasquerade";

        public string ImageSource => "";

        public string LogoSource => "";

        public string NewCharacter()
        {
            throw new NotImplementedException();
        }
    }
}
