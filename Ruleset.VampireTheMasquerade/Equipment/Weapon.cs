using Utility;

namespace VampireTheMasquerade
{
    public class Weapon : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int Damage { get; set; }
    }
}
