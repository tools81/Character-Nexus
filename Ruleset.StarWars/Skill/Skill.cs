using Utility;

namespace StarWars
{
    internal class Skill : ISkill
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Attribute { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Value { get; set; }
        public int Max { get; set; } = 5;
    }
}
