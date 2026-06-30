using Utility;

namespace StarWars
{
    internal class Armor : IEquipment
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Models { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int Defense { get; set; }
        public int Soak { get; set; }
        public int Encumberance { get; set; }
        public int HP { get; set; }
        public int Price { get; set; }
        public int Rarity { get; set; }
    }
}

