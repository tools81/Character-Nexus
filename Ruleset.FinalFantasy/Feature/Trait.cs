using Utility;

namespace FinalFantasy
{
    internal class Trait : IFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int? Limitation { get; set; }
    }
}

