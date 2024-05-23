using EverydayHeroes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace EverydayHeroes
{
    public class Item : IEquipment
    {
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public int price { get; set; }
        public int bulk { get; set; }
    }
}
