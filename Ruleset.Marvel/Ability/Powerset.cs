using Utility;

namespace Marvel
{
    public class Powerset : IBaseJson
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? imageUrl { get; set; }
    }
}