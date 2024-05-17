using System.Collections.Generic;

namespace Utility
{
    public class UserChoice<T>
    {
        public string Type { get; set; }
        public List<T> Choices { get; set; }
        public int Count { get; set; }
    }
}