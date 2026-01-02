using Utility;

namespace VampireTheMasquerade
{
    public class Power : IAbility
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public string Cost { get; set; }
        public string DicePool { get; set; }
        public string System { get; set; }
        public string Duration { get; set; }
        public string Discipline { get; set; }
    }
}
