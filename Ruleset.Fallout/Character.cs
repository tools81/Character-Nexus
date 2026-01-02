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

        public CharacterSegment CharacterSegment => throw new NotImplementedException();

        public string CharacterSheet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public byte[] BuildCharacterSheet()
        {
            throw new NotImplementedException();
        }
    }
}
