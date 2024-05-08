using EverydayHeroes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Interfaces;

namespace EverydayHeroes
{
    public class Ability : IAbility
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AbilityType Type { get; set; }

    }
}
