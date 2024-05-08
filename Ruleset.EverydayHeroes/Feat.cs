using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Interfaces;

namespace EverydayHeroes
{
    public class Feat : IAbility
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
