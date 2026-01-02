using System.Collections.Generic;
using Utility;

namespace VampireTheMasquerade
{
    public class Clan : IFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<string> Disciplines { get; set; }
        public string Compulsion { get; set; }
        public string Bane { get; set; }
    }
}
