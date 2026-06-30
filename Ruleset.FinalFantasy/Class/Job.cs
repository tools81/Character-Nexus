using Utility;

namespace FinalFantasy
{
    internal class Job : IClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public List<BonusAdjustment>? BonusAdjustments { get; set; }
    }
}
