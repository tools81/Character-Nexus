using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverydayHeroes
{
    public class Class
    {
        public string Description { get; set; }
        public int SkillProficiencyCount { get; set; }
        public List<Proficiency> Profeciencies { get; set; }
        public List<List<Ability>> Talents { get; set; }
    }
}
