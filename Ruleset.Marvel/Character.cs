using System.Reflection;
using Utility;

namespace Marvel
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string RealName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int Rank { get; set; }
        public string Height { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Eyes { get; set; } = string.Empty;
        public string Hair { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string DistinguishingFeatures { get; set; } = string.Empty;
        public string Teams { get; set; } = string.Empty;
        public string Base { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string History { get; set; } = string.Empty;
        public string Personality { get; set; } = string.Empty;
        public List<Attribute> Attributes { get; set; } = new List<Attribute>();
        public Occupation Occupation { get; set; } = new Occupation();
        public Origin Origin { get; set; } = new Origin();
        public List<Power> Powers { get; set; } = new List<Power>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<Trait> Traits { get; set; } = new List<Trait>();
        public int Health { get; set; }
        public int HealthDamageReduction { get; set; }
        public int Focus { get; set; }
        public int FocusDamageReduction { get; set; }
        public int Run { get; set; }
        public int Climb { get; set; }
        public int Swim { get; set; }
        public int Karma { get; set; }
        public int InitiativeModifier { get; set; }
        public List<Weapon> Weapons { get; set; } = new List<Weapon>();
        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }
        public string CharacterSheet { get; set; } = string.Empty;        

        public byte[] BuildCharacterSheet()
        {
            var dict = new Dictionary<string, string>
            {
                { "Codename", Name },
                { "Real Name", RealName },
                { "Rank", Rank.ToString() },
                { "Height", Height },
                { "Weight", Weight },
                { "Gender", Gender },
                { "Eyes", Eyes },
                { "Hair", Hair },
                { "Size", Size },
                { "Distinguishing Features", DistinguishingFeatures },
                { "Teams", Teams },
                { "Base", Base }
            };

            List<string> notesArray = Notes.DivideStringIntoWordArray(20);
            for (int i = 0; i < notesArray.Count; i++)
            {
                if (i < 8) //Maximum number of lines on character sheet
                {
                    dict.Add($"NOTES {i + 1}", notesArray[i]);
                }
            }

            List<string> historyArray = History.DivideStringIntoWordArray(20);
            for (int i = 0; i < historyArray.Count; i++)
            {
                if (i < 9) //Maximum number of lines on character sheet
                {
                    dict.Add($"HISTORY {i + 1}", historyArray[i]);
                }
            }

            List<string> personalityArray = Personality.DivideStringIntoWordArray(20);
            for (int i = 0; i < personalityArray.Count; i++)
            {
                if (i < 9) //Maximum number of lines on character sheet
                {
                    dict.Add($"PERSONALITY {i + 1}", personalityArray[i]);
                }
            }

            var attribute = Attributes.Where(a => a.Name == "Melee").FirstOrDefault();
            if (attribute is not null)
            {
                dict.Add("Melee Score", attribute.Value.ToString());
                dict.Add("Melee Defense", attribute.Defense.ToString());
                dict.Add("Melee Non-Combat", attribute.Check.ToString());
                dict.Add("Melee DMG Multiplier", attribute.Damage.ToString());
                dict.Add("Melee DMG Ability Score", attribute.Value.ToString());
            }

            attribute = Attributes.Where(a => a.Name == "Agility").FirstOrDefault();
            if (attribute is not null)
            {
                dict.Add("Agility Score", attribute.Value.ToString());
                dict.Add("Agility Defense", attribute.Defense.ToString());
                dict.Add("Agility Non-Combat", attribute.Check.ToString());
                dict.Add("Agility DMG Multiplier", attribute.Damage.ToString());
                dict.Add("Agility DMG Ability Score", attribute.Value.ToString());
            }

            attribute = Attributes.Where(a => a.Name == "Resilience").FirstOrDefault();
            if (attribute is not null)
            {
                dict.Add("Resilience Score", attribute.Value.ToString());
                dict.Add("Resilience Defense", attribute.Defense.ToString());
                dict.Add("Resilience Non-Combat", attribute.Check.ToString());
                dict.Add("Resilience DMG Multiplier", attribute.Damage.ToString());
                dict.Add("Resilience DMG Ability Score", attribute.Value.ToString());
            }

            attribute = Attributes.Where(a => a.Name == "Vigilance").FirstOrDefault();
            if (attribute is not null)
            {
                dict.Add("Vigilance Score", attribute.Value.ToString());
                dict.Add("Vigilance Defense", attribute.Defense.ToString());
                dict.Add("Vigilance Non-Combat", attribute.Check.ToString());
                dict.Add("Vigilance DMG Multiplier", attribute.Damage.ToString());
                dict.Add("Vigilance DMG Ability Score", attribute.Value.ToString());
            }

            attribute = Attributes.Where(a => a.Name == "Ego").FirstOrDefault();
            if (attribute is not null)
            {
                dict.Add("Ego Score", attribute.Value.ToString());
                dict.Add("Ego Defense", attribute.Defense.ToString());
                dict.Add("Ego Non-Combat", attribute.Check.ToString());
                dict.Add("Ego DMG Multiplier", attribute.Damage.ToString());
                dict.Add("Ego DMG Ability Score", attribute.Value.ToString());
            }

            attribute = Attributes.Where(a => a.Name == "Logic").FirstOrDefault();
            if (attribute is not null)
            {
                dict.Add("Logic Score", attribute.Value.ToString());
                dict.Add("Logic Defense", attribute.Defense.ToString());
                dict.Add("Logic Non-Combat", attribute.Check.ToString());
                dict.Add("Logic DMG Multiplier", attribute.Damage.ToString());
                dict.Add("Logic DMG Ability Score", attribute.Value.ToString());
            }

            dict.Add("Occupation", Occupation.Name);
            dict.Add("Profession", Occupation.Name);
            dict.Add("Origin", Origin.Name);

            for (int i = 0; i < Powers.Count; i++)
            {
                if (i < 25) //Maximum lines on character sheet
                {
                    dict.Add($"Power_{i + 1}", Powers[i].Name);
                    dict.Add($"Set_{i + 1}", Powers[i].Powersets[0]);
                    dict.Add($"Cost_{i + 1}", Powers[i].Cost ?? string.Empty);
                    dict.Add($"Page_{i + 1}", Powers[i].Page.ToString());

                    var desc = Powers[i].Description ?? string.Empty;
                    var descLength = desc.Length;

                    dict.Add($"Summary_{i + 1}", descLength > 120 ? desc.Substring(0, 120) : desc);                    

                    switch (Powers[i].Duration)
                    {
                        case DurationType.Permanent:
                            dict.Add($"Duration Permanent {i + 1}", "true");
                            break;
                        case DurationType.Instant:
                            dict.Add($"Duration Instant {i + 1}", "true");
                            break;
                        case DurationType.OneRound:
                            dict.Add($"Duration 1 Round {i + 1}", "true");
                            break;
                        case DurationType.Concentration:
                            dict.Add($"Duration Concentration {i + 1}", "true");
                            break;
                        default:
                            break;
                    }

                    foreach (var action in Powers[i].Actions ?? Enumerable.Empty<ActionType>())
                    {
                        switch (action)
                        {
                            case ActionType.Standard:
                                dict.Add($"Action Standard {i + 1}", "true");
                                break;
                            case ActionType.Movement:
                                dict.Add($"Action Movement {i + 1}", "true");
                                break;
                            case ActionType.Reaction:
                                dict.Add($"Action Reaction {i + 1}", "true");
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            for (int i = 0; i < Tags.Count; i++)
            {
                if (i < 16) //Maximum lines on character sheet
                {
                    dict.Add($"TAGS {i + 1}", Tags[i].Name);
                }
            }

            for (int i = 0; i < Traits.Count; i++)
            {
                if (i < 14) //Maximum lines on character sheet
                {
                    dict.Add($"TRAITS {i + 1}", Traits[i].Name);
                }
            }

            for (int i = 0; i < Weapons.Count; i++)
            {
                if (i < 2) //Maximum lines on character sheet
                {
                    dict.Add($"WEAPONS {i + 1}", Weapons[i].Name);
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

            return PDFSchema.Generate(dict, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Marvel_Character_Sheet.pdf");
        }

        private int GetAttributeValue(string attribute)
        {
            var selected = Attributes.FirstOrDefault(a => a.Name == attribute);

            if (selected is not null)
            {
                return selected.Value;
            }
            else
            {
                return 1;
            }
        }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                Image = Image,
                Level = Rank,
                LevelName = "Rank",
                Details = $"{Origin?.Name} | {Occupation?.Name}",
                CharacterSheet = CharacterSheet
            };
        }
    }
}