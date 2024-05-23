using EverydayHeroes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace EverydayHeroes
{
    public class Weapon : IEquipment
    {
        public string Description { get; set; }
        public EquipmentTier Tier { get; set; }
        public WeaponCategory Category { get; set; }
        public string Damage { get; set; }
        public string Range { get; set; }
        public int Penetration { get; set; }
        public List<string> Properties { get; set; }
        public int Price { get; set; }
        public int Bulk { get; set; }
    }
}
