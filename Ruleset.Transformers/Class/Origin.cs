using System.Collections.Generic;
using Utility;

namespace Transformers
{
    public class Origin : IClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Health { get; set; }
        public string Benefit { get; set; }
        public string BenefitDescription { get; set; }
        public List<BonusAdjustment> BonusAdjustments { get; set; }
        public List<UserChoice> UserChoices { get; set; }
        public string BotMovement { get; set; }
        public string BotSize { get; set; }
        public string BotFineMotor { get; set; }
        public string AltMovement { get; set; }
        public string AltSize { get; set; }
        public int AltCrew { get; set; }
        public string AltFirepoints { get; set; }
        public int AltAttack { get; set; }
        public string Languages { get; set; }
    }
}