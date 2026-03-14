using System.Collections.Generic;
using Utility;

namespace Transformers
{
    public class Focus : IClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public List<BonusAdjustment> BonusAdjustments { get; set; }
        public List<BonusCharacteristic> BonusCharacteristics { get; set; }
        public List<UserChoice> UserChoices { get; set; }
        //TODO: Do something with levels
    }
}