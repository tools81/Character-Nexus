using System.Collections.Generic;
using Utility;

namespace Marvel
{
    public class Trait : IBaseJson
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BonusAdjustment>? BonusAdjustments { get; set; }
    }
}