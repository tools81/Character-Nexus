using Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiJoe
{
    public class Character : ICharacter
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
