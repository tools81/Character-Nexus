using Utility;

namespace StarWars
{
    internal class ItemAttachment : IEquipment
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Models { get; set; } = string.Empty;
        public int Damage { get; set; }
        public List<string> Qualities { get; set; } = new List<string>();
        public List<ItemMod> ModSets { get; set; } = new List<ItemMod>();
        public int RequiredHP { get; set; }
        public int Price { get; set; }
        public int Rarity { get; set; }
    }
}
