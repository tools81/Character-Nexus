using System.Collections.Generic;
using Utility;

namespace EverydayHeroes
{
    public class Weapon : IEquipment, IBaseJson
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tier { get; set; }
        public string Category { get; set; }
        public string Damage { get; set; }
        public string Range { get; set; }
        public int Penetration { get; set; }
        public List<string> Properties { get; set; }
        public int Price { get; set; }
        public int Bulk { get; set; }
        public string Image { get; set; }
    }
}
