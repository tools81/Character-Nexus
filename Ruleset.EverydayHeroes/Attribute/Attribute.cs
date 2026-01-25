using Utility;

namespace EverydayHeroes
{
    public class Attribute : IAttribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public int Modifier { get => GetModifierValue(); }
        public string Image { get; set; }

        private int GetModifierValue()
        {
            return (Value - 10) / 2;
        }
    }
}
