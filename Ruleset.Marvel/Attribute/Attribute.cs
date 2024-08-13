using System;
using Utility;

namespace Marvel
{
    public class Attribute : IAttribute, IBaseJson
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Value { get; set; }
        public int Defense { get; set; }       
        public int Check { get; set; }       
        public int Damage { get; set; }
    }
}