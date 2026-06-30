using Utility;

namespace StarWars
{
    internal class Career : IClass
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Specializations { get; set; } = new List<string>();
        public List<BonusCharacteristic> BonusCharacteristics { get; set; } = new List<BonusCharacteristic>();        
        public List<UserChoice> UserChoices { get; set; } = new List<UserChoice>();
    }
}
