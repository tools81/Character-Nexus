using Utility;

namespace StarWars
{
    internal class ItemMod : IEquipment
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Qualities { get; set; } = new List<string>();
        public List<string> Talents { get; set; } = new List<string>();
        public List<string> Skill { get; set; } = new List<string>();
        public List<string> Talent { get; set; } = new List<string>();
    }
}
