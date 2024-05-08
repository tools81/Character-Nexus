using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.Interfaces
{
    public interface IAttribute
    {
        public string Description { get; set; }
        public int Value { get; set; }
     }
}
