using Utility;

namespace FinalFantasy
{
    internal class Role : IClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Jobs { get; set; }
        public string Image { get; set; }
    }
}