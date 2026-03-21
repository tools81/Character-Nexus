using Utility;

namespace CallOfCthulhu
{
    public class Equipment : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Cost { get; set; }
        public string Era { get; set; }
    }
}