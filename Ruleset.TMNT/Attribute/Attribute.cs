using Utility;

namespace TMNT
{
    public class Attribute : IAttribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public int Value { get; set; }
    }
}