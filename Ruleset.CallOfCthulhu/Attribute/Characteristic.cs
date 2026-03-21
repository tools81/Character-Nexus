using Utility;

namespace CallOfCthulhu
{
    public class Characteristic : IAttribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Roll { get; set; }
        public int Value { get; set; }
    }
}