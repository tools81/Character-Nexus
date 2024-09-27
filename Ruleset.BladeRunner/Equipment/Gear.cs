﻿using Utility;

namespace BladeRunner
{
    public class Gear : IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Effect { get; set; }
        public string Availability { get; set; }
        public int Cost { get; set; }
    }
}