using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazingTales
{
    public class Attribute : IAttribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
    }
}
