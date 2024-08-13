using System;

namespace Utility
{
    public class BonusAdjustment<T> where T : Enum
    {
        public T Type { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
    }
}