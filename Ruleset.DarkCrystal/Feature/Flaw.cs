namespace DarkCrystal
{
    public class Flaw : IFeature, IBaseJson
    {
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}