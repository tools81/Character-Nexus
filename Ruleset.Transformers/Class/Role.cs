using System.Collections.Generic;
using Utility;

namespace Transformers
{
    public class Role : IClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BonusAdjustment> BonusAdjustments { get; set; }
        public List<BonusCharacteristic> BonusCharacteristics { get; set; }
        public List<UserChoice> UserChoices { get; set; }
        //TODO: Do something with levels, armorupgrades, and weapons
    }
}