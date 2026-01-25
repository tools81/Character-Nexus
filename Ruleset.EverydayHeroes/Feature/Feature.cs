using System.Collections.Generic;
using Utility;

namespace EverydayHeroes
{
    public class Feature : IFeature
    {
        public List<Background> Backgrounds { get; set; }
        public List<Profession> Professions { get; set; }
        public List<Feat> Feats { get; set; }
    }
}
