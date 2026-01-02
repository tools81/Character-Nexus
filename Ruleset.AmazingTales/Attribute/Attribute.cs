using Utility;

namespace AmazingTales
{
    public class Attribute : IAttribute, IBaseJson
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
    }
}
