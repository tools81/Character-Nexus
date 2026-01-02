using System.Collections.Generic;
using Utility;

namespace VampireTheMasquerade
{
    public class Generation : IFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BonusAdjustment> BonusAdjustments { get; set; }
    }
}
