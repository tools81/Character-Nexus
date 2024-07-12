using Utility;

namespace VampireTheMasquerade
{
    public class Merit : IFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Advantage { get; set; }
        public int Range { get; set; }
        public int Value { get; set; }
    }
}
