using Utility;

namespace TMNT
{
    public class Vehicle : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
    }
}