using System.Collections.Generic;
using Utility;

namespace DarkCrystal
{
    public class Clan : IFeature, IBaseJson
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<BonusCharacteristic> BonusCharacteristics { get; set; } = new List<BonusCharacteristic>();
        public List<UserChoice> UserChoices { get; set; } = new List<UserChoice>();
        public string? image { get; set; }
    }
}