using Utility;

namespace DarkCrystal
{
    public class Specialization : ISkill, IBaseJson
    {
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Master { get; set; }
        public string Skill { get; set; } = string.Empty;
    }
}