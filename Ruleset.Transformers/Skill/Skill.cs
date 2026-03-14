using Utility;

namespace Transformers
{
    public class Skill : ISkill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
    }
}