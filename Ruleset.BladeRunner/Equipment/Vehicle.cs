using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace BladeRunner
{
    public class Vehicle : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Passengers { get; set; }
        public string Maneuverability { get; set; }
        public int Hull { get; set; }
        public string Armor { get; set; }
        public string Availability { get; set; }
        public int Cost { get; set; }
    }
}
