using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace EverydayHeroes
{
    public class Archetype : IClass
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public string HitDice { get; set; }
        public int Hitpoints { get; set; }
        public int HitpointsPerLevel { get; set; }
        public string HitpointModifier { get; set; }
        public string DefenseModifier { get; set; }
        public List<int> ProficiencyBonus { get; set; }
        public List<int> DefenseBonus { get; set; }     
        public List<List<string>> Talents { get; set; }
    }
}
