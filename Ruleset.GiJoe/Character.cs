using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GiJoe
{
    public class Character : ICharacter
    {
        public string Name { get; set; }
        public FileInfo Image { get; set; }
        public string ImageUrl { get; set; }
        public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CharacterSegment CharacterSegment => throw new NotImplementedException();
    }
}
