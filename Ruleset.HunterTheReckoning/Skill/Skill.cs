using System.Collections.Generic;
using Utility;

namespace HunterTheReckoning
{
    public class Skill : ISkill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Aspect { get; set; }
        public int Value { get; set; }
    }
}
