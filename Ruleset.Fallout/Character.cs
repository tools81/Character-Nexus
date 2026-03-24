using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Fallout
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string Image { get; set; } = string.Empty;
        public int CarryWeight { get; set; }
        public int DamageResistance { get; set; }
        public int Defense { get; set; }
        public int Initiative { get; set; }
        public int HealthPoints { get; set; }
        public int MeleeDamage { get; set; }
        public Origin Origin { get; set; }
        public Trait Trait { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Trait> Traits { get; set; }
        public List<Perk> Perks { get; set; }
        public Pack Pack { get; set; }
        public List<Clothing> Clothings { get; set; }
        public List<Armor> Armors { get; set; }
        public List<ArmorRobot> RobotArmors { get; set; }
        public List<Consumeable> Consumeables { get; set; }
        public List<Item> Items { get; set; }
        public List<Weapon> Weapons { get; set; }

        public CharacterSegment CharacterSegment => throw new NotImplementedException();

        public string CharacterSheet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public byte[] BuildCharacterSheet()
        {
            throw new NotImplementedException();
        }
    }
}
