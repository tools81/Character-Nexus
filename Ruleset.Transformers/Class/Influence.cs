using Utility;

namespace Transformers
{
    public class Influence : IClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Perk { get; set; }
        public string Suggestion { get; set; }
        //TODO: Deal with hangups and bonds
    }
}