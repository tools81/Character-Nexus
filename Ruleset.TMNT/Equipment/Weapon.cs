using Utility;

namespace TMNT
{
    public class Weapon : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool TwoHanded { get; set; }
        public string Length { get; set; }
        public string Weight { get; set; }
        public string Damage { get; set; }
        public int Cost { get; set; }
        public string Cartridge { get; set; }
        public string Feed { get; set; }
        public string Range { get; set; }
    }
}