using Utility;

namespace DarkCrystal
{
    public class Gender : IFeature, IBaseJson
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}