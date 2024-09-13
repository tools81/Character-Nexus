using System.Reflection;
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
        public string? CharacterSheet { get; set; }

        public void SetBonusAdjustments()
        {
            var bonuses = new HashSet<BonusAdjustment>();

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

            //foreach (var bonus in bonuses.ToList())
            //{
            //    switch (bonus.Type)
            //    {
            //        case BonusType.AttributeValue:
            //            Attributes.FirstOrDefault(a => a.Name == bonus.Name).Value += bonus.Value;
            //            break;
            //        case BonusType.AttributeDamage:
            //            Attributes.FirstOrDefault(a => a.Name == bonus.Name).Damage += bonus.Value;
            //            break;
            //        case BonusType.AttributeDefense:
            //            Attributes.FirstOrDefault(a => a.Name == bonus.Name).Defense += bonus.Value;
            //            break;
            //        case BonusType.AttributeCheck:
            //            Attributes.FirstOrDefault(a => a.Name == bonus.Name).Check += bonus.Value;
            //            break;
            //        case BonusType.Health:
            //            Health += bonus.Value;
            //            break;
            //        case BonusType.Focus:
            //            Focus += bonus.Value;
            //            break;
            //        case BonusType.HealthDamageReduction:
            //            HealthDamageReduction += bonus.Value;
            //            break;
            //        case BonusType.FocusDamageReduction:
            //            FocusDamageReduction += bonus.Value;
            //            break;
            //        case BonusType.Run:
            //            Run += bonus.Value;
            //            break;
            //        case BonusType.Climb:
            //            Climb += bonus.Value;
            //            break;
            //        case BonusType.Swim:
            //            Swim += bonus.Value;
            //            break;
            //        case BonusType.Karma:
            //            Karma += bonus.Value;
            //            break;
            //        case BonusType.InitiativeModifier:
            //            InitiativeModifier += bonus.Value;
            //            break;
            //        default:
            //            break;
            //    }
            //}

            Health += Attributes.FirstOrDefault(a => a.Name == "Resilience").Value * 30;
            Focus += Attributes.FirstOrDefault(a => a.Name == "Vigilance").Value * 30;
            InitiativeModifier += Attributes.FirstOrDefault(a => a.Name == "Vigilance").Value;
            Run += 5 + (Attributes.FirstOrDefault(a => a.Name == "Agility").Value / 5);
            Climb += Run / 2;
            Swim += Run / 2;
            Karma += Rank;
        }

        public byte[] GetCharacterSheet()
        {
            var dict = new Dictionary<string, string>();

            dict.Add("Codename", Name);
            dict.Add("Real Name", RealName);
            dict.Add("Rank", Rank.ToString());
            dict.Add("Height", Height);
            dict.Add("Weight", Weight);
            dict.Add("Gender", Gender);
            dict.Add("Eyes", Eyes);
            dict.Add("Hair", Hair);
            dict.Add("Size", Size);
            dict.Add("Distinguishing Features", DistinguishingFeatures);
            dict.Add("Teams", Teams);
            dict.Add("Base", Base);
            dict.Add("Notes", Notes);

            List<string> historyArray = History.DivideStringIntoWordArray(20);
            for (int i = 0; i < historyArray.Count; i++)
            {
                if (i < 9) //Maximum number of lines on character sheet
                {
                    dict.Add($"History [{i}]", historyArray[i]);
                }
            }

            List<string> personalityArray = Personality.DivideStringIntoWordArray(20);
            for (int i = 0; i < personalityArray.Count; i++)
            {
                if (i < 9) //Maximum number of lines on character sheet
                {
                    dict.Add($"Personality [{i}]", personalityArray[i]);
                }
            }

            var attribute = Attributes.Where(a => a.Name == "Melee").FirstOrDefault();
            dict.Add("Melee Score", attribute.Value.ToString());
            dict.Add("Melee Defense", attribute.Defense.ToString());
            dict.Add("Melee Non-Combat", attribute.Check.ToString());
            dict.Add("Melee DMG Multiplier", attribute.Damage.ToString());
            dict.Add("Melee DMG Ability Score", attribute.Value.ToString());

            attribute = Attributes.Where(a => a.Name == "Agility").FirstOrDefault();
            dict.Add("Agility Score", attribute.Value.ToString());
            dict.Add("Agility Defense", attribute.Defense.ToString());
            dict.Add("Agility Non-Combat", attribute.Check.ToString());
            dict.Add("Agility DMG Multiplier", attribute.Damage.ToString());
            dict.Add("Agility DMG Ability Score", attribute.Value.ToString());

            attribute = Attributes.Where(a => a.Name == "Resilience").FirstOrDefault();
            dict.Add("Resilience Score", attribute.Value.ToString());
            dict.Add("Resilience Defense", attribute.Defense.ToString());
            dict.Add("Resilience Non-Combat", attribute.Check.ToString());
            dict.Add("Resilience DMG Multiplier", attribute.Damage.ToString());
            dict.Add("Resilience DMG Ability Score", attribute.Value.ToString());

            attribute = Attributes.Where(a => a.Name == "Vigilance").FirstOrDefault();
            dict.Add("Vigilance Score", attribute.Value.ToString());
            dict.Add("Vigilance Defense", attribute.Defense.ToString());
            dict.Add("Vigilance Non-Combat", attribute.Check.ToString());
            dict.Add("Vigilance DMG Multiplier", attribute.Damage.ToString());
            dict.Add("Vigilance DMG Ability Score", attribute.Value.ToString());

            attribute = Attributes.Where(a => a.Name == "Ego").FirstOrDefault();
            dict.Add("Ego Score", attribute.Value.ToString());
            dict.Add("Ego Defense", attribute.Defense.ToString());
            dict.Add("Ego Non-Combat", attribute.Check.ToString());
            dict.Add("Ego DMG Multiplier", attribute.Damage.ToString());
            dict.Add("Ego DMG Ability Score", attribute.Value.ToString());

            attribute = Attributes.Where(a => a.Name == "Logic").FirstOrDefault();
            dict.Add("Logic Score", attribute.Value.ToString());
            dict.Add("Logic Defense", attribute.Defense.ToString());
            dict.Add("Logic Non-Combat", attribute.Check.ToString());
            dict.Add("Logic DMG Multiplier", attribute.Damage.ToString());
            dict.Add("Logic DMG Ability Score", attribute.Value.ToString());

            dict.Add("Occupation", Occupation.Name);
            dict.Add("Origin", Origin.Name);

            for (int i = 0; i < Powers.Count; i++)
            {
                if (i < 24) //Maximum lines on character sheet
                {
                    dict.Add($"Power [{i}]", Powers[i].Name);
                    dict.Add($"Set [{i}]", Powers[i].Powersets[0]);
                    dict.Add($"Cost [{i}]", Powers[i].Cost);
                    dict.Add($"Page [{i}]", Powers[i].Page.ToString());

                    var descLength = Powers[i].Description.Length;

                    dict.Add($"Summary [{i}]", descLength > 120 ? Powers[i].Description.Substring(0, 120) : Powers[i].Description);

                    switch (Powers[i].Duration)
                    {
                        case DurationType.Permanent:
                            dict.Add($"Duration Permanent [{i}]", "true");
                            break;
                        case DurationType.Instant:
                            dict.Add($"Duration Instant [{i}]", "true");
                            break;
                        case DurationType.OneRound:
                            dict.Add($"Duration 1 Round [{i}]", "true");
                            break;
                        case DurationType.Concentration:
                            dict.Add($"Duration Concentration [{i}]", "true");
                            break;
                        default:
                            break;
                    }

                    foreach (var action in Powers[i].Actions ?? Enumerable.Empty<ActionType>())
                    {
                        switch (action)
                        {
                            case ActionType.Standard:
                                dict.Add($"Action Standard [{i}]", "true");
                                break;
                            case ActionType.Movement:
                                dict.Add($"Action Movement [{i}]", "true");
                                break;
                            case ActionType.Reaction:
                                dict.Add($"Action Reaction [{i}]", "true");
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            for (int i = 0; i < Tags.Count; i++)
            {
                if (i < 15) //Maximum lines on character sheet
                {
                    dict.Add($"Tags [{i}]", Tags[i].Name);
                }
            }

            for (int i = 0; i < Traits.Count; i++)
            {
                if (i < 18) //Maximum lines on character sheet
                {
                    dict.Add($"Traits [{i}]", Traits[i].Name);
                }
            }

            dict.Add("Health", Health.ToString());
            dict.Add("Health DR", HealthDamageReduction.ToString());
            dict.Add("Focus", Focus.ToString());
            dict.Add("Focus DR", FocusDamageReduction.ToString());
            dict.Add("Run Speed", Run.ToString());
            dict.Add("Climb Speed", Climb.ToString());
            dict.Add("Swim Speed", Swim.ToString());
            dict.Add("Karma", Karma.ToString());
            dict.Add("Initiative Modifier", InitiativeModifier.ToString());

            //TODO: Figure out where to put Weapons

            return PDFSchema.Generate(dict, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Character_Sheet.pdf");
        }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                Image = Image,
                Level = Rank,
                Details = $"{Origin.Name} | {Occupation.Name}"
            };
        }
    }
}