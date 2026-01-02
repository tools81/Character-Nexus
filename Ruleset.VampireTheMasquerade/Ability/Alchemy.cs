using Utility;

namespace VampireTheMasquerade
{
    public class Alchemy : IAbility
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Cost { get; set; }
        public string DicePool { get; set; }
        public string System { get; set; }
        public string Duration { get; set; }
        public int Level { get; set; }
    }
}
