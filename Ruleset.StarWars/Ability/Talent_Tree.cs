using Utility;

namespace StarWars
{
    internal class Talent_Tree : IAbility
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int Cost { get; set; }
        public bool Active { get; set; }
        public bool Force { get; set; }
        public Tuple<int, int>? Coordinates { get; set; }
        public List<Tuple<int, int>> Connections { get; set; } = new List<Tuple<int, int>>();
    }
}
