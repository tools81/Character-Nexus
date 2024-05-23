using EverydayHeroes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace EverydayHeroes
{
    public class Vehicle : IEquipment
    {
        public string Description { get; set; }
        public VehicleTier Tier { get; set; }
        public VehicleDomain Domain { get; set; }
        public string Passengers { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int ArmorValue { get; set; }
        public string Speed { get; set; }
        public List<string> Properties { get; set; }
        public int Price { get; set; }
    }
}
