using System.Collections.Generic;
using Utility;

namespace EverydayHeroes
{
    public class Class
    {
        public string Name { get; set; }
        public string Archetype { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<BonusAdjustment>? BonusAdjustments { get; set; }
        public List<BonusCharacteristic>? BonusCharacteristics { get; set; }
        public List<UserChoice>? UserChoices { get; set; }
        public List<List<string>> TalentPerLevel { get; set; }
    }
}
