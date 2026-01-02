namespace DarkCrystal
{
    public class Gear : IEquipment, IBaseJson
    {
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Durability { get; set; } = string.Empty;
    }
}