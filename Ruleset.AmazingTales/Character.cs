using Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace AmazingTales
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string D12Attribute { get; set; }
        public string D10Attribute { get; set; }
        public string D8Attribute { get; set; }
        public string D6Attribute { get; set; }
        public string Notes { get; set; }

        public CharacterSegment CharacterSegment {get => GetCharacterSegment(); }
        public string? CharacterSheet { get; set; }

        public byte[] BuildCharacterSheet()
        {
            var dict = new Dictionary<string, string>();

            dict.Add("Name", Name);
            dict.Add("D12Attribute", D12Attribute);
            dict.Add("D10Attribute", D10Attribute);
            dict.Add("D8Attribute", D8Attribute);
            dict.Add("D6Attribute", D6Attribute);
            dict.Add("Notes", Notes);

            return PDFSchema.Generate(dict, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Amazing_Tales_Character_Sheet.pdf");
        }


        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                Image = Image
            };
        }
    }
}
