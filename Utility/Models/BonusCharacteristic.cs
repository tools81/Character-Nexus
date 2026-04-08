using System.Collections.Generic;

namespace Utility
{
    public class BonusCharacteristic
    {
        public string Type { get; set; } = "";
        public string Value { get; set; } = "";
        public List<BonusCondition>? Conditions { get; set; } = new List<BonusCondition>();
    }
}