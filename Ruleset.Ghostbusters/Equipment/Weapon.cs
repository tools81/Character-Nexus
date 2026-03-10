using Utility;

namespace Ghostbusters
{
    public class Weapon : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Hands { get; set; }
        public string Muscles { get; set; }
        public string Maximum { get; set; }
        public string Increment { get; set; }
        public string ToHit { get; set; }
        public string Damage { get; set; }
    }
}