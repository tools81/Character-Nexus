using System;
using System.Collections.Generic;

namespace Utility
{
    public class UserChoice
    {
        public string Type { get; set; }
        public List<string> Choices { get; set; }
        public int Count { get; set; }
        public object Value { get; set; }
    }
}