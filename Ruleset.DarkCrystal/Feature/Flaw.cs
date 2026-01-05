using Utility;

namespace DarkCrystal
{
    public class Flaw : IFeature, IBaseJson
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}