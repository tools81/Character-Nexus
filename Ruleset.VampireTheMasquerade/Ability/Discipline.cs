using System.Collections.Generic;
using Utility;

namespace VampireTheMasquerade
{
    public class Discipline : IAbility
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AspectType Aspect { get; set; }
        public string Threat { get; set; }
        public string Resonance { get; set; }
        public int Value { get; set; }
    }
}
