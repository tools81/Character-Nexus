using Utility;

namespace VampireTheMasquerade
{
    public class Ritual : IAbility
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Ingredients { get; set; }
        public string Process { get; set; }
        public string System { get; set; }
        public int Level { get; set; }
    }
}
