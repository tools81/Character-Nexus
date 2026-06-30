using Utility;

namespace StarWars
{
    internal class ItemQuality : IEquipment
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Value { get; set; }
    }
}
