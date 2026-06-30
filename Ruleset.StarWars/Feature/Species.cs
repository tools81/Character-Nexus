using Utility;

namespace StarWars
{
    internal class Species : IFeature
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Physiology { get; set; } = string.Empty;
        public string Society { get; set; } = string.Empty;
        public string Homeworld { get; set; } = string.Empty;
        public string Abilities { get; set; } = string.Empty;
        public int Wound { get; set; }
        public int Strain { get; set; }
        public int Experience { get; set; }
        public List<BonusAdjustment> BonusAdjustments { get; set; } = new List<BonusAdjustment>();
        public List<BonusCharacteristic> BonusCharacteristics { get; set; } = new List<BonusCharacteristic>();
        public List<UserChoice> UserChoices { get; set; } = new List<UserChoice>();
    }
}
