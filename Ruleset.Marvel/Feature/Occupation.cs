using System.Collections.Generic;
using Utility;

namespace Marvel
{
    public class Occupation : IFeature, IBaseJson
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<BonusCharacteristic>? BonusCharacteristics { get; set; }
    }
}