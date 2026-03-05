using Utility;

namespace BladeRunner
{
    public class Specialty : IFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BonusAdjustment>? BonusAdjustments { get; set; }
    }
}
