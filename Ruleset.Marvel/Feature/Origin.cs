using System.Collections.Generic;
using Utility;

namespace Marvel
{
    public class Origin : IFeature, IBaseJson
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Traits { get; set; }
        public List<string>? Tags { get; set; }
    }
}