using System.Collections.Generic;
using Utility;

namespace TMNT
{
    public class Animal : IFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Size { get; set; }
        public string Length { get; set; }
        public string Weight { get; set; }
        public string Build { get; set; }
        public int Bio { get; set; }
        public List<BonusAdjustment> BonusAdjustments { get; set; }
    }
}