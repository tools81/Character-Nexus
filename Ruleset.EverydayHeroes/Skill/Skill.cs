using Utility;

namespace EverydayHeroes
{
    public class Skill : ISkill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AbilityModifier { get; set; }
        public bool Proficient { get; set; }
        public bool Expertise { get; set; }
    }
}
