using Utility;

namespace TMNT
{
    public class Skill : ISkill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Value { get; set; }
    }
}