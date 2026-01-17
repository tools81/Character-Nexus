using Utility;

namespace Marvel
{
    public class Weapon : IEquipment, IBaseJson
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Range { get; set; }
        public string? Multiplier { get; set; }
    }
}