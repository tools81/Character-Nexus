using Utility;

namespace Marvel
{
    public class Tag : IFeature, IBaseJson
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}