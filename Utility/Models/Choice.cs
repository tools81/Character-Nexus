using System;
using System.Collections.Generic;

namespace Utility
{
    public class Choice
    {
        public Dictionary<string, Dictionary<string, List<object?>>> Sections { get; set; }
        = new Dictionary<string, Dictionary<string, List<object?>>>();
    }
}