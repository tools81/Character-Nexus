using Utility;

namespace FinalFantasy
{
    internal class Item : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public string Base { get; set; }
        public string Tier { get; set; }
        public int Price { get; set; }
        public string? Range { get; set; }
        public string Target { get; set; }
    }
}

