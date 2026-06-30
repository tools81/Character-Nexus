using Utility;

namespace FinalFantasy
{
    internal class LimitBreak : IAbility
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Trigger { get; set; }
        public string Effect { get; set; }
        public List<string> Types { get; set; }
    }
}

