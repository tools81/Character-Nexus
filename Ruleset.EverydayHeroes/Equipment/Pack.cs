using System.Collections.Generic;
using Utility;

namespace EverydayHeroes
{
    public class Pack : IEquipment, IBaseJson
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public List<BonusCharacteristic>? BonusCharacteristics { get; set; }
    }
}
