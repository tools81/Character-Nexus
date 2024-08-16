using Utility;

namespace Marvel
{
    public class CharacterForm
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? RealName { get; set; }
        public string? Image { get; set; }
        public int Rank { get; set; }
        public string? Height { get; set; }
        public string? Weight { get; set; }
        public string? Gender { get; set; }
        public string? Eyes { get; set; }
        public string? Hair { get; set; }
        public string? Size { get; set; }
        public string? DistinguishingFeatures { get; set; }
        public string? Teams { get; set; }
        public string? Base { get; set; }
        public string? Notes { get; set; }
        public string? History { get; set; }
        public string? Personality { get; set; }
        public List<FormInt>? Attributes { get; set; }
        public string? Occupation { get; set; }
        public string? Origin { get; set; }
        public List<FormBool>? Powers { get; set; }
        public List<string>? Tags { get; set; }
        public List<string>? Traits { get; set; }
        public List<string>? Weapons { get; set; } 
    }
}