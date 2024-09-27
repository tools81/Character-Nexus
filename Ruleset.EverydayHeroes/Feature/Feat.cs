using EverydayHeroes.Enums;
using System;
using System.Collections.Generic;
using Utility;

namespace EverydayHeroes
{
    public class Feat
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Prerequisite { get; set; }
        public FeatType Type { get; set; }
        public FeatScale Scale { get; set; }
        public List<BonusAdjustment>? BonusAdjustments { get; set; }
        public List<BonusCharacteristic>? BonusCharacteristics { get; set; }
        public List<Prerequisite<object>>? Prerequisites { get; set; }
        public List<UserChoice>? UserChoices { get; set; }
    }
}
