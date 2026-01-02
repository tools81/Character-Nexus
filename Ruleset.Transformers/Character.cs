using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Transformers
{
    internal class Character : ICharacter
    {
        public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Image { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CharacterSegment CharacterSegment => throw new NotImplementedException();

        public string CharacterSheet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public byte[] BuildCharacterSheet()
        {
            throw new NotImplementedException();
        }
    }
}
