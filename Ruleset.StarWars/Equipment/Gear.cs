using Utility;

namespace StarWars
{
    internal class Gear : IEquipment
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Models { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Encumbrance { get; set; }
        public int Rarity { get; set; }
    }
}
