using System.Collections.Generic;
using Utility;

namespace EverydayHeroes
{
    public class Vehicle : IEquipment, IBaseJson
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tier { get; set; }
        public string Domain { get; set; }
        public string Passengers { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int ArmorValue { get; set; }
        public string Speed { get; set; }
        public List<string> Properties { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
    }
}
