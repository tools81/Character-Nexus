using Utility;

namespace StarWars
{
    internal class Talent : IAbility
    {
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<BonusAdjustment> BonusAdjustments { get; set; } = new List<BonusAdjustment>();
        public List<UserChoice> UserChoices { get; set; } = new List<UserChoice>();
    }
}
