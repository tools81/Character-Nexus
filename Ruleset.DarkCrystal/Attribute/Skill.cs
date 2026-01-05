using Utility;

namespace DarkCrystal
{
    public class Skill : ISkill, IBaseJson
    {
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Trained { get; set; }
    }
}