using EverydayHeroes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace EverydayHeroes
{
    public class Ability : IAbility
    {
        public Dictionary<string, Talent> Talents { get; set; }
        public Dictionary<string, Plan> Plans { get; set; }
        public Dictionary<string, Trick> Tricks { get; set; }
    }
}
