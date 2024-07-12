using System.Collections.Generic;
using Utility;

namespace VampireTheMasquerade
{
    public class Skill : ISkill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AspectType Aspect { get; set; }
        public List<string> Specialties { get; set; }
    }
}
