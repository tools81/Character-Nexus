using Utility;

namespace Marvel
{
    public class Weapon : IEquipment, IBaseJson
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Range { get; set; }
        public string Multiplier { get; set; }
    }
}