using Utility;

namespace CallOfCthulhu
{
    public class Skill : ISkill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public string Filter { get; set; }
    }
}