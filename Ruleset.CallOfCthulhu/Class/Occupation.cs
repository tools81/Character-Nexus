using System.Collections.Generic;
using Utility;

namespace CallOfCthulhu
{
    public class Occupation : IClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Credit { get; set; }
        public string SuggestedContacts { get; set; }
        public string SkillPoints { get; set; }
        public List<string> Skills { get; set; }
        public int SpecialtyCount { get; set; }
    }
}