using System.Collections.Generic;
using Utility;

namespace EverydayHeroes
{
    public class Ability : IAbility
    {
        public List<Talent> Talents { get; set; }
        public List<Plan> Plans { get; set; }
        public List<Trick> Tricks { get; set; }
    }
}
