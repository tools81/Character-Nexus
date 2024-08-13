using System.Collections.Generic;
using Utility;

namespace Marvel
{
    public class Power : IAbility, IBaseJson
    {
        public required string Name { get; set; }
        public List<string>? Powersets { get; set; }
        public string? Description { get; set; }
        public int Page { get; set; }
        public List<ActionType>? Actions { get; set; }
        public DurationType Duration { get; set; }
        public string? Trigger { get; set; }
        public string? Range { get; set; }
        public string? Effect { get; set; }
        public string? Cost { get; set; }
        public Prerequisite<int>? PrerequisitesInt { get; set; }
        public Prerequisite<string>? PrerequisitesString { get; set; }
        public List<BonusAdjustment<BonusType>>? BonusAdjustments { get; set; }
    }
}