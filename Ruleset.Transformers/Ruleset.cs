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

        public ICharacter NewCharacter()
        {
            throw new NotImplementedException();
        }
    }
}
