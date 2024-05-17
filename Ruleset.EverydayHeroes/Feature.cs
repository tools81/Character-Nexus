using EverydayHeroes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace EverydayHeroes
{
    public class Feature : IFeature
    {
        public Dictionary<string, Background> Backgrounds { get; set; }
        public Dictionary<string, Profession> Professions { get; set; }
        public Dictionary<string, Feat> Feats { get; set; }
    }
}
