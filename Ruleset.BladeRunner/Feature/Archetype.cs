using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace BladeRunner
{
    public class Archetype : IFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Origins { get; set; }
        public string KeyAttribute { get; set; }
        public string KeySkills { get; set; }
        public string KeyFavoredGear { get; set; }
        public string KeySpecialties { get; set; }
        public List<BonusAdjustment> BonusAdjustments { get; set; }
    }
}
