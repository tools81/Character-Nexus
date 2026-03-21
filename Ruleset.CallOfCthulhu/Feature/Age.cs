using System.Collections.Generic;
using Utility;

namespace CallOfCthulhu
{
    public class Age : IFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BonusAdjustment> BonusAdjustments { get; set; }
        public List<UserChoice> UserChoices { get; set; }
    }
}