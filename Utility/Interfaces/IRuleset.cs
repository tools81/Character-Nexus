using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility
{
    public interface IRuleset
    {
        public string Name { get; }
        public string RulesetName { get; }
        public string ImageSource { get; }
        public string LogoSource { get; }
        public string NewCharacter();
    }
}
