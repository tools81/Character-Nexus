using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverydayHeroes
{
    public class Attribute : IAttribute
    {
        public string Description { get; set; }
        public int Value { get; set; }
        public int Modifier { get => GetModifierValue(); }

        private int GetModifierValue()
        {
            return (Value - 10) / 2;
        }
    }
}
