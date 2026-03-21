using System.Collections.Generic;
using Utility;

namespace CallOfCthulhu
{
    public class Weapon : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Skill { get; set; }
        public string Damage { get; set; }
        public string Range { get; set; }
        public string Uses { get; set; }
        public string Magazine { get; set; }
        public string Cost { get; set; }
        public int Malfunction { get; set; }
        public List<string> Eras { get; set; }
    }
}