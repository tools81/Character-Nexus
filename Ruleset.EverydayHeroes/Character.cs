using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverydayHeroes
{
    public class Character : ICharacter
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Level { get; set; }
        public Archetype Archetype { get; set; }
        public Class Class { get; set; }
        public Background Background { get; set; }
        public Profession Profession { get; set; }
        public Dictionary<string, Attribute> Attributes { get; set; }
        public Dictionary<string, Skill> Skills { get; set; }
        public Dictionary<string, Talent> Talents { get; set; }
        public Dictionary<string, Feat> Feats { get; set; }
        public int Speed { get => 30; }
        public string HitDice { get => GetHitDice(); }
        public int HitPoints { get => GetHitPoints(); }
        public int Defense { get => GetDefense(); }
        public bool HasDamageReduction { get; set; }
        public int DamageReduction { get => GetDamageReduction(); }
        public int ProficiencyBonus { get => GetProficiencyBonus(); }
        public int PassivePerception { get => GetPassivePerception(); }  
        public int WealthLevel { get => GetWealthLevel(); } 
        public int GeniusPoints { get; set; }    
        public int FocusPoints { get; set; }
        public int InfluencePoints { get; set; }
        public int LuckPoints { get; set; }

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
            return Archetype.Hitpoints + ((Archetype.HitpointsPerLevel + Attributes["Constitution"].Modifier) * Level);
        }

        private int GetDefense()
        {
            return 10 + Attributes[Archetype.DefenseModifier].Modifier + Archetype.DefenseBonus[Level];
        }

        private int GetDamageReduction()
        {
            return HasDamageReduction ? GetProficiencyBonus() : 0;
        }

        private int GetPassivePerception()
        {
            var perceptionSkill = Skills["Perception"];
            var value = 10 + Attributes["Wisdom"].Modifier + Attributes[perceptionSkill.AbilityModifier].Modifier;

            if (perceptionSkill.Procifient)
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
    }
}
