using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace EverydayHeroes
{
    public class Class
    {
        public string Description { get; set; }
        public List<UserChoice<string>> Profeciencies { get; set; }
        public List<List<Talent>> Talents { get; set; }
    }
}
