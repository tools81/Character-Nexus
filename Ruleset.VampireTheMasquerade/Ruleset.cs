using Utility.Interfaces;
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

        public ICharacter NewCharacter()
        {
            throw new NotImplementedException();
        }
    }
}
