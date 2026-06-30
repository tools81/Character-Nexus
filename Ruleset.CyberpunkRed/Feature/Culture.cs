using System.Collections.Generic;
using Utility;

namespace CyberpunkRed
{
    internal class Culture : IFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserChoice> UserChoices { get; set; }
    }
}
