using Utility;

namespace FinalFantasy
{
    internal class Augmentation : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string? Trigger { get; set; }
        public int Limitation { get; set; }
        public List<BonusCharacteristic> BonusCharacteristics { get; set; } = new List<BonusCharacteristic>();
    }
}