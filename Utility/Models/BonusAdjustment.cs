using System;
using System.Collections.Generic;

namespace Utility
{
    public class BonusAdjustment
    {
        public string Type { get; set; } = "";
        public string Name { get; set; } = "";
        public int Value { get; set; } = 0;
        public List<BonusCondition>? Conditions { get; set; } = new List<BonusCondition>();
    }
}