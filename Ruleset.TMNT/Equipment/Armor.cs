using Utility;

namespace TMNT
{
    public class Armor : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int AR { get; set; }
        public int SDC { get; set; }
        public string Weight { get; set; }
        public int Cost { get; set; }
    }
}