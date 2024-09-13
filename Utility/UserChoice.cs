using System;
using System.Collections.Generic;

namespace Utility
{
    public class UserChoice<T> where T : Enum
    {
        public T Type { get; set; }
        public List<string> Choices { get; set; }
        public int Count { get; set; }
    }
}