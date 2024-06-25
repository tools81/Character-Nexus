using Utility;

namespace Marvel
{
    public class Power
    {
        public string Name { get; set; }
        public Powerset[] Powersets { get; set; }
        public string Description { get; set; }
        public int Page { get; set; }
        public ActionType[] Actions { get; set; }
        public DurationType Duration { get; set; }
        public string Trigger { get; set; }
        public string Range { get; set; }
        public string Effect { get; set; }
        public string Cost { get; set; }
        public Prerequisite<int> PrerequisitesInt { get; set; }
        public Prerequisite<string> PrerequisitesString { get; set; }
    }
}