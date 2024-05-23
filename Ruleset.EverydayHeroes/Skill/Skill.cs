using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace EverydayHeroes
{
    public class Skill : ISkill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AbilityModifier { get; set; }
        public bool Procifient { get; set; }
        public bool Expertise { get; set; }
    }
}
