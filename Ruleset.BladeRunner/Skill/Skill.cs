using Utility;

namespace BladeRunner
{
    public class Skill : ISkill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Attribute { get; set; }
        public int Value { get; set; }
    }
}
