using Utility;

namespace DarkCrystal
{
    public class Trait : IFeature, IBaseJson
    {
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Clan { get; set; }
    }
}
