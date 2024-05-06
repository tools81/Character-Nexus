using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IAttribute
    {
        public string Name { get; }

        public int Value { get; set; }
     }
}
