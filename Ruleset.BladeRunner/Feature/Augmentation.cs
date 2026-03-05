using Utility;

namespace BladeRunner
{
    public class Augmentation : IFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Availability { get; set; }
        public string Cost { get; set; }
    }
}
