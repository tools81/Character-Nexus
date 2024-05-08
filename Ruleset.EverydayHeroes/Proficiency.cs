using EverydayHeroes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Interfaces;

namespace EverydayHeroes
{
    public class Proficiency
    {
        public ProficiencyType Type { get; set; }
        public List<string> Targets { get; set; }
    }
}
