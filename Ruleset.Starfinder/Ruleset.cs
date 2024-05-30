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

        public string ImageSource => "https://drive.google.com/thumbnail?id=11NZo8Ij3XDbHjFh4n5YHtKFyQdRFIGJh&sz=w1024";

        public string LogoSource => "https://drive.google.com/thumbnail?id=1nEcGzQLVUcO7qJRKo6DXbzCDN7tGlZey&sz=256";

        public ICharacter NewCharacter()
        {
            throw new NotImplementedException();
        }
    }
}
