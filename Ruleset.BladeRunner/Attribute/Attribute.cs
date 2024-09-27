using System;
using Utility;

namespace BladeRunner
{
    public class Attribute : IAttribute
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Value { get; set; }
    }
}