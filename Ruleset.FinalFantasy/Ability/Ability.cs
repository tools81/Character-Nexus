using Utility;

namespace FinalFantasy
{
    internal class Ability : IAbility
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string? Image { get; set; }
        public string? Target { get; set; }
        public string? Trigger { get; set; }
        public string? Range { get; set; }
        public string? Check { get; set; }
        public string? CR { get; set; }
        public string? Base { get; set; }
        public string? Direct { get; set; }
        public int Limitation { get; set; }
    }
}

