using EverydayHeroes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace EverydayHeroes
{
    public class Armor : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public EquipmentTier Tier { get; set; }
        public ArmorCategory Category { get; set; }
        public int Value { get; set; }
        public List<string> Properties { get; set; }
        public int Price { get; set; }
        public int Bulk { get; set; }
    }
}
