using Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EverydayHeroes
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Level { get; set; }
        public string Languages { get; set; }
        public string Motivations { get; set; }
        public string Attachments { get; set; }
        public string Beliefs { get; set; }
        public string Ancestry { get; set; }
        public string Quirks { get; set; }
        public string Virtues { get; set; }
        public string Flaws { get; set; }
        public string Role { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string Hair { get; set; }
        public string Skin { get; set; }
        public string Eyes { get; set; }
        public string Age { get; set; }
        public string MaritalStatus { get; set; }
        public string Pronouns { get; set; }
        public string Biography { get; set; }
        public string Notes { get; set; }
        public Archetype Archetype { get; set; }
        public Class Class { get; set; }
        public Background Background { get; set; }
        public Profession Profession { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Talent> Talents { get; set; }
        public List<Feat> Feats { get; set; }
        public List<string> EquipmentProficiency { get; set; }
        public int Speed { get => 30; }
        public string HitDice { get => GetHitDice(); }
        public int HitPoints { get => GetHitPoints(); }
        public int Defense { get => GetDefense(); }
        public int Initiative { get => GetInitiative(); }
        public bool HasDamageReduction { get; set; }
        public int DamageReduction { get => GetDamageReduction(); }
        public int ProficiencyBonus { get => GetProficiencyBonus(); }
        public int PassivePerception { get => GetPassivePerception(); }  
        public int WealthLevel { get => GetWealthLevel(); } 
        public int GeniusPoints { get; set; }    
        public int FocusPoints { get; set; }
        public int InfluencePoints { get; set; }
        public int LuckPoints { get; set; }
        public bool Inspiration { get; set; }
        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }
        public string CharacterSheet { get; set; }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment() {
                Id = Id,
                Name = Name,
                Image = Image,
                Level = Level,
                LevelName = "Level",
                Details = $"{Archetype} | {Class}",
                CharacterSheet = CharacterSheet
            };
        }

        private int GetProficiencyBonus()
        {
            return Archetype.ProficiencyBonus[Level];
        }

        private string GetHitDice()
        {
            return $"{Level}{Archetype.HitDice}";
        }

        private int GetHitPoints()
        {
            return Archetype.Hitpoints + ((Archetype.HitpointsPerLevel + Attributes.Where(a => a.Name == "Constitution").FirstOrDefault().Modifier) * Level);
        }

        private int GetDefense()
        {
            return 10 + Attributes.Where(a => a.Name == Archetype.DefenseModifier).FirstOrDefault().Modifier + Archetype.DefenseBonus[Level];
        }

        private int GetInitiative()
        {
            return Attributes.Where(a => a.Name == "Dexterity").FirstOrDefault().Modifier;
        }

        private int GetDamageReduction()
        {
            return HasDamageReduction ? GetProficiencyBonus() : 0;
        }

        private int GetPassivePerception()
        {
            var perceptionSkill = Skills.Where(s => s.Name == "Perception").FirstOrDefault();
            var value = 10 + Attributes.Where(a => a.Name == "Wisdom").FirstOrDefault().Modifier + Attributes.Where(a => a.Name == perceptionSkill.AbilityModifier).FirstOrDefault().Modifier;

            if (perceptionSkill.Proficient)
            {
                if (perceptionSkill.Expertise)
                {
                    value += ProficiencyBonus * 2;
                }
                else
                {
                    value += ProficiencyBonus;
                }
            }

            return value;
        }

        private int GetWealthLevel()
        {
            return Profession.WealthLevel;
        }

        public byte[] BuildCharacterSheet()
        {
            throw new NotImplementedException();
        }
    }
}
