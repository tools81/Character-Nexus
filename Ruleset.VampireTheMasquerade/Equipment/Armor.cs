using Utility;

namespace VampireTheMasquerade
{
    public class Armor : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Physical { get; set; }
        public int Ballistic { get; set; }
    }
}
