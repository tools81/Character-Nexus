using Utility;

namespace WorldWideWrestling
{
    public class Role : IFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Advanced { get; set; }
    }
}