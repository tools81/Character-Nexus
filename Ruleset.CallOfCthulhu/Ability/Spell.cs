using Utility;

namespace CallOfCthulhu
{
    public class Spell : IAbility
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Cost { get; set; }
        public string Time { get; set; }
    }
}