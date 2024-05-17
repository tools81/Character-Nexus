using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EverydayHeroes.Enums;
using Utility;

namespace EverydayHeroes
{
    public class Profession
    {
        public string Description { get; set; }
        public string SampleCareers { get; set; }
        public List<UserChoice<string>> AttributeBonuses { get; set; }
        public List<UserChoice<string>> Profeciencies { get; set; }
        public string IconicEquipment { get; set; }
        public string Language { get; set; }
        public int WealthLevel { get; set; }
        public string SpecialFeature { get; set; }
    }
}
