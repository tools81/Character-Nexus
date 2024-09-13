using EverydayHeroes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace EverydayHeroes
{
    public class Feat
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Prerequisite { get; set; }
        public FeatType Type { get; set; }
        public FeatScale Scale { get; set; }
        public List<BonusAdjustment> BonusAdjustments { get; set; }
        //public List<UserChoice<string>> AttributeBonuses { get; set; }
        //public List<UserChoice<string>> Abilities { get; set; }
        //public List<UserChoice<string>> Profeciencies { get; set; }   
    }
}
