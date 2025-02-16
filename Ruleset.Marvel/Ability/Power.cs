using System.Collections.Generic;
using Utility;

namespace Marvel
{
    public class Power : IAbility, IBaseJson
    {
        public required string Name { get; set; }
        public List<string> Powersets { get; set; } = new List<string>();
        public string? Description { get; set; }
        public int Page { get; set; }
        public List<ActionType>? Actions { get; set; }
        public DurationType Duration { get; set; }
        public string? Trigger { get; set; }
        public string? Range { get; set; }
        public string? Effect { get; set; }
        public string? Cost { get; set; }
        public List<Prerequisite>? Prerequisites { get; set; }
        public List<BonusAdjustment>? BonusAdjustments { get; set; }
    }
}