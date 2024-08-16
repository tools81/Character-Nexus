using Utility;

namespace Marvel
{
    public class Character : ICharacter
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
        public List<Attribute>? Attributes { get; set; }
        public Occupation? Occupation { get; set; }
        public Origin? Origin { get; set; }
        public List<Power>? Powers { get; set; }
        public List<Tag>? Tags { get; set; }
        public List<Trait>? Traits { get; set; }
        public int Health { get; set; }       
        public int HealthDamageReduction { get; set; }
        public int Focus { get; set; }
        public int FocusDamageReduction { get; set; }
        public int Run { get; set; }
        public int Climb { get; set; }
        public int Swim { get; set; }
        public int Karma { get; set; }
        public int InitiativeModifier { get; set; }
        public List<Weapon>? Weapons { get; set; } 
        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                ImageUrl = Image,
                Level = Rank,
                Details = $"{Origin.Name} | {Occupation.Name}"
            };
        }

        public void SetBonusAdjustments()
        {
            var bonuses = new HashSet<BonusAdjustment<BonusType>>();

            foreach (var trait in Traits)
            {
                if (trait.BonusAdjustments != null)
                {
                    foreach (var bonus in trait.BonusAdjustments)
                    {
                        bonuses.Add(bonus);
                    }
                }
            }

            foreach (var power in Powers)
            {
                if (power.BonusAdjustments != null)
                {
                    foreach (var bonus in power.BonusAdjustments)
                    {
                        bonuses.Add(bonus);
                    }
                }
            }

            foreach (var attribute in Attributes)
            {
                attribute.Check = attribute.Value;
                attribute.Defense = 10 + attribute.Value;
                attribute.Damage = attribute.Value;
            }

            foreach (var bonus in bonuses.ToList())
            {
                switch (bonus.Type)
                {
                    case BonusType.AttributeValue:
                        Attributes.FirstOrDefault(a => a.Name == bonus.Name).Value += bonus.Value;
                        break;
                    case BonusType.AttributeDamage:
                        Attributes.FirstOrDefault(a => a.Name == bonus.Name).Damage += bonus.Value;
                        break;
                    case BonusType.AttributeDefense:
                        Attributes.FirstOrDefault(a => a.Name == bonus.Name).Defense += bonus.Value;
                        break;
                    case BonusType.AttributeCheck:
                        Attributes.FirstOrDefault(a => a.Name == bonus.Name).Check += bonus.Value;
                        break;
                    case BonusType.Health:
                        Health += bonus.Value;
                        break;
                    case BonusType.Focus:
                        Focus += bonus.Value;
                        break;
                    case BonusType.HealthDamageReduction:
                        HealthDamageReduction += bonus.Value;
                        break;
                    case BonusType.FocusDamageReduction:
                        FocusDamageReduction += bonus.Value;
                        break;
                    case BonusType.Run:
                        Run += bonus.Value;
                        break;
                    case BonusType.Climb:
                        Climb += bonus.Value;
                        break;
                    case BonusType.Swim:
                        Swim += bonus.Value;
                        break;
                    case BonusType.Karma:
                        Karma += bonus.Value;
                        break;
                    case BonusType.InitiativeModifier:
                        InitiativeModifier += bonus.Value;
                        break;
                    default:
                        break;
                }
            }

            Health += Attributes.FirstOrDefault(a => a.Name == "Resilience").Value * 30;
            Focus += Attributes.FirstOrDefault(a => a.Name == "Vigilance").Value * 30;
            InitiativeModifier += Attributes.FirstOrDefault(a => a.Name == "Vigilance").Value;
            Run += 5 + (Attributes.FirstOrDefault(a => a.Name == "Agility").Value / 5);
            Climb += Run / 2;
            Swim += Run / 2;
            Karma += Rank;
        }
    }
}