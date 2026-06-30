using Utility;

namespace StarWars
{
    internal class Weapon : IEquipment
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Models { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Skill { get; set; } = string.Empty;
        public int Damage { get; set; }
        public int Critical { get; set; }
        public string Range { get; set; } = string.Empty;
        public int Encumberance { get; set; }
        public int HP { get; set; }
        public int Price { get; set; }
        public int Rarity { get; set; }
        public List<Tuple<string, int>> Qualities { get; set; } = new List<Tuple<string, int>>();
    }
}
