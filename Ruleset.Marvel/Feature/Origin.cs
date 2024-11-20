using System.Collections.Generic;
using Utility;

namespace Marvel
{
    public class Origin : IFeature, IBaseJson
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<BonusCharacteristic> BonusCharacteristics { get; set; } = new List<BonusCharacteristic>();
    }
}