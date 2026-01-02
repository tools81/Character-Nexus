namespace DarkCrystal
{
    public class Trait : IAttribute, IBaseJson
    {
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Clan { get; set; }
    }
}
