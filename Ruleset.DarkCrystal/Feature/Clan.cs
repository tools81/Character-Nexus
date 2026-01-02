using Utility;

namespace DarkCrystal
{
    public class Clan : IFeature, IBaseJson
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<BonusCharacteristic> BonusCharacteristics { get; set; } = new List<BonusCharacteristic>();
        public UserChoice UserChoice { get; set; } = new UserChoice();
        public string? image { get; set; }
    }
}