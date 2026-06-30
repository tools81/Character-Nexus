using Utility;

namespace HunterTheReckoning
{
    public class Attribute : IAttribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Aspect { get; set; }
        public int Value { get; set; }
    }
}
