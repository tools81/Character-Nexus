using Utility;

namespace Marvel
{
    public class Origin : IBaseJson
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Traits { get; set; }
        public List<string> Tags { get; set; }
    }
}