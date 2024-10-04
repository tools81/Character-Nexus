using System.Collections.Generic;
using Utility;

namespace VampireTheMasquerade
{
    public class Predator : IFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserChoice> UserChoices { get; set; }
        public List<BonusAdjustment> BonusAdjustments { get; set; }
    }
}
