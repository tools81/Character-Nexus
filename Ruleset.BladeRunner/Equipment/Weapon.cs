using Utility;

namespace BladeRunner
{
    public class Weapon : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Damage { get; set; }
        public string CritDie { get; set; }
        public string Type { get; set; }
        public string MinRange { get; set; }
        public string MaxRange { get; set; }
        public string Availability { get; set; }
        public int Cost { get; set; }
    }
}
