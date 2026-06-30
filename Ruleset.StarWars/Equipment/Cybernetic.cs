using Utility;

namespace StarWars
{
    internal class Cybernetic : IEquipment
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Rarity { get; set; }
        public List<BonusAdjustment> BonusAdjustments { get; set; } = new List<BonusAdjustment>();
    }
}
