using System.Collections.Generic;
using Utility;

namespace WorldWideWrestling
{
    public class Gimmick : IClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Injury { get; set; }
        public List<BonusAdjustment> BonusAdjustments { get; set; }
        public List<BonusCharacteristic> BonusCharacteristics { get; set; }
        public List<UserChoice> UserChoices { get; set; }
    }
}