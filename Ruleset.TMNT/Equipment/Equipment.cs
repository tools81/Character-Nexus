using Utility;

namespace TMNT
{
    public class Equipment : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public string Type { get; set; }
    }
}