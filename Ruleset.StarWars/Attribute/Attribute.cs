using Utility;

namespace StarWars
{
    internal class Attribute : IAttribute
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Value { get; set; }
    }
}
