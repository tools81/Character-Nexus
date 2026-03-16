using Utility;

namespace WorldWideWrestling
{
    public class Stat : IAttribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}