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
        public List<Background> Backgrounds { get; set; }
        public List<Profession> Professions { get; set; }
        public List<Feat> Feats { get; set; }
    }
}
