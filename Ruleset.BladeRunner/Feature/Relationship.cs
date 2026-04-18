using Utility;

namespace BladeRunner
{
    public class Relationship : IFeature
    {
        public string Who { get; set; }
        public string What { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
