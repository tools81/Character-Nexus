using Utility;

namespace FinalFantasy
{
    internal class Race : IFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<string> Subraces { get; set; }
        public string History { get; set; }
    }
}
