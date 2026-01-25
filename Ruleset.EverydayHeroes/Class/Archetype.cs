using System.Collections.Generic;
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
        public string DefenseModifier { get; set; }
        public List<int> DefenseBonusPerLevel { get; set; }     
        public List<List<string>> TalentsPerLevel { get; set; }
    }
}
