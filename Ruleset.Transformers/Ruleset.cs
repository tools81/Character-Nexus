using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transformers
{
    public class Ruleset : IRuleset
    {
        public string Name => "Transformers";

        public string RulesetName => "Ruleset.Transformers";

        public string ImageSource => "https://drive.google.com/thumbnail?id=1bxmZ9cw8-FOVLsmU4TuJJKfHrAslyWCm&sz=w1024";

        public string LogoSource => "https://drive.google.com/thumbnail?id=1dn3tVNJU5JCDomJSvPoSLjLIGYU2oIWb&sz=w256";

        public ICharacter NewCharacter()
        {
            throw new NotImplementedException();
        }
    }
}
