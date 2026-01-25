using System.Collections.Generic;
using Utility;

namespace EverydayHeroes
{
    public class Background
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconicEquipment { get; set; }
        public string Language { get; set; }
        public string SpecialFeature { get; set; }
        public List<BonusAdjustment>? BonusAdjustments { get; set; }
        public List<BonusCharacteristic>? BonusCharacteristics { get; set; }
        public List<UserChoice>? UserChoices { get; set; }
    }
}
