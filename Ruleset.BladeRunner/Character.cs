using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Utility;

namespace BladeRunner
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Appearance { get; set; }
        public string Image { get; set; }
        public int Health { get; set; }
        public int Resolve { get; set; }       
        public int Chinyen { get; set; }
        public int PromotionPoints { get; set; }
        public int HumanityPoints { get; set; }
        public string Notes { get; set; }
        public Origin? Origin { get; set; }
        public Archetype? Archetype { get; set; }
        public List<Attribute>? Attributes { get; set; }
        public Tenure? Tenure { get; set; }
        public List<Skill>? Skills { get; set; }
        public List<Specialty>? Specialties { get; set; }
        public Memory? Memory { get; set; }
        public Relationship? Relationship { get; set; }
        public string FavoredGear { get; set; }
        public string SignatureItem { get; set; }
        public string Home { get; set; }
        public List<Gear>? Gears { get; set; }
        public List<Armor>? Armors { get; set; }
        public List<Weapon>? Weapons { get; set; }
        public List<Vehicle>? Vehicles { get; set; }

        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }
        public string CharacterSheet { get; set; }

        public byte[] BuildCharacterSheet()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("Name", Name);

            List<string> appearanceArray = Appearance.DivideStringIntoWordArray(20);
            for (int i = 0; i < appearanceArray.Count; i++)
            {
                if (i < 3) //Maximum number of lines on character sheet
                {
                    dict.Add($"Appearance [{i}]", appearanceArray[i]);
                }
            }

            dict.Add("Appearance", Appearance);
            dict.Add("Health", Health.ToString());
            dict.Add("Resolve", Resolve.ToString());

            for (int i = 0; i < Chinyen && i < 20; i++)
            {
                dict.Add($"Chinyen [{i}]", "true");
            }

            for (int i = 0; i < PromotionPoints && i < 20; i++)
            {
                dict.Add($"PromotionPoints [{i}]", "true");
            }

            for (int i = 0; i < HumanityPoints && i < 20; i++)
            {
                dict.Add($"HumanityPoints [{i}]", "true");
            }

            dict.Add("Notes", Notes);
            dict.Add("Origin", Origin.Name);
            dict.Add("Archetype", Archetype.Name);
            dict.Add("FavoredGear", FavoredGear.Length > 90 ? FavoredGear.Substring(0, 90) : FavoredGear);
            dict.Add("SignatureItem", SignatureItem.Length > 90 ? SignatureItem.Substring(0, 90) : SignatureItem);

            List<string> homeArray = Home.DivideStringIntoWordArray(20);
            for (int i = 0; i < homeArray.Count; i++)
            {
                if (i < 2) //Maximum number of lines on character sheet
                {
                    dict.Add($"Home [{i}]", homeArray[i]);
                }
            }

            var attribute = Attributes.Where(a => a.Name == "Strength").FirstOrDefault();
            dict.Add("StrengthRating", attribute.Value.ToString());

            attribute = Attributes.Where(a => a.Name == "Agility").FirstOrDefault();
            dict.Add("AgilityRating", attribute.Value.ToString());

            attribute = Attributes.Where(a => a.Name == "Intelligence").FirstOrDefault();
            dict.Add("IntelligenceRating", attribute.Value.ToString());

            attribute = Attributes.Where(a => a.Name == "Empathy").FirstOrDefault();
            dict.Add("EmpathyRating", attribute.Value.ToString());

            dict.Add("Tenure", Tenure.Description);

            for (int i = 0; i < Skills.Count; i++)
            {
                dict.Add($"{Skills[i]}Rating", Skills[i].Value.ToString());
            }

            for (int i = 0; i < Specialties.Count; i++)
            {
                dict.Add($"Specialty [{i}]", Specialties[i].Name);
            }

            dict.Add("MemoryWhen", Memory.When);
            dict.Add("MemoryWhere", Memory.Where);
            dict.Add("MemoryWhat", Memory.What);
            dict.Add("MemoryWho", Memory.Who);
            dict.Add("MemoryHow", Memory.How);

            dict.Add("RelationshipWho", Relationship.Who);
            dict.Add("RelationshipWhat", Relationship.What);
            dict.Add("RelationshipStatus", Relationship.Status);

            for (int i = 0; i < Gears.Count; i++)
            {
                if (i < 7) //Maximum lines on character sheet
                {
                    dict.Add($"Gear [{i}]", Gears[i].Name);
                }
            }

            for (int i = 0; i < Armors.Count; i++)
            {
                if (i < 1) //Maximum lines on character sheet
                {
                    dict.Add($"Armor [{i}]", Armors[i].Name);
                }
            }

            for (int i = 0; i < Weapons.Count; i++)
            {
                if (i < 3) //Maximum lines on character sheet
                {
                    dict.Add($"Weapon [{i}]", Weapons[i].Name);
                }
            }

            for (int i = 0; i < Vehicles.Count; i++)
            {
                if (i < 1) //Maximum lines on character sheet
                {
                    dict.Add($"Vehicle [{i}]", Vehicles[i].Name);
                }
            }

            return PDFSchema.Generate(dict, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Blade_Runner_Character_Sheet.pdf");
        }


        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                Image = Image,
                Details = $"{Origin.Name} | {Archetype.Name} | {Tenure.Name}"
            };
        }
    }
}
