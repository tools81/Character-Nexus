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
        public List<Talent> Talents { get; set; }
        public List<Plan> Plans { get; set; }
        public List<Trick> Tricks { get; set; }
    }
}
