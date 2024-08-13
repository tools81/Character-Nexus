using Utility;

namespace Marvel
{
    public class Tag : IFeature, IBaseJson
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}