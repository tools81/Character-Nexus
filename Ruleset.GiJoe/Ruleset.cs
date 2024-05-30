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

        public string ImageSource => "https://drive.google.com/thumbnail?id=1hEU9DPJ8zfCx2CjmWkS2MtVtLlUBbs9g&sz=w1024";

        public string LogoSource => "https://drive.google.com/thumbnail?id=1HlnlGG-jkcRF3F7pM9pVpdENTAc0BstT&sz=w256";

        public ICharacter NewCharacter()
        {
            throw new NotImplementedException();
        }
    }
}
