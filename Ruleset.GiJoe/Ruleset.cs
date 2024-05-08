using Utility.Interfaces;
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

        public ICharacter NewCharacter()
        {
            throw new NotImplementedException();
        }
    }
}
