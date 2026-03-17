using System.Collections.Generic;
using Utility;

namespace TMNT
{
    public class Education : IFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BonusAdjustment> BonusAdjustments { get; set; }
    }
}