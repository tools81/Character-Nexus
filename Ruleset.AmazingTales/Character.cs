using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace AmazingTales
{
    public class Character : ICharacter
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public Dictionary<string, Attribute> Attributes { get; set; }
    }
}
