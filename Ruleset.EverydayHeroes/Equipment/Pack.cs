using EverydayHeroes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace EverydayHeroes
{
    public class Pack : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Clothes { get; set; }
        public int Price { get; set; }
        public List<string> Items { get; set; }
        public List<string> Armors { get; set; }
        public List<string> Weapons { get; set; }
        public List<string> Vehicles { get; set; }
    }
}
