using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Interfaces;

namespace EverydayHeroes
{
    public class Archetype : IClass
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public string HitDice { get; set; }
        public int Hitpoints { get; set; }
        public Attribute HitpointModifier { get; set; }
        public int Defense { get; set; }
        public Attribute DefenseModifier { get; set; }
        public List<int> ProficiencyBonus { get; set; }
        public List<int> DefenseBonus { get; set; }     
        public List<List<Ability>> Talents { get; set; }
        public List<Class> Classes { get; set; }
    }
}
