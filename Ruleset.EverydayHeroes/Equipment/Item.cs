using Utility;

namespace EverydayHeroes
{
    public class Item : IEquipment, IBaseJson
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int price { get; set; }
        public int bulk { get; set; }
        public string Image { get; set; }
    }
}
