using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Utility
{
    public interface ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public FileInfo Image { get; set; }
        public string ImageUrl { get; set; }       
        public CharacterSegment CharacterSegment { get; }
    }
}
