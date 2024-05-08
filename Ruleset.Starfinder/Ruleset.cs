using Utility.Interfaces;
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

        public ICharacter NewCharacter()
        {
            throw new NotImplementedException();
        }
    }
}
