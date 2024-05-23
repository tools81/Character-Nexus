using EverydayHeroes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace EverydayHeroes
{
    public class Equipment : IEquipment
    {
        public Dictionary<string, Armor> Armors { get; set; }
        public Dictionary<string, Item> Items { get; set; }
        public Dictionary<string, Pack> Packs { get; set; }
        public Dictionary<string, Vehicle> Vehicles { get; set; }
        public Dictionary<string, Weapon> Weapons { get; set; }
    }
}
