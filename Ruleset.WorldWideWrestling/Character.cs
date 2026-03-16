using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Utility;

namespace WorldWideWrestling
{
    public class Character : ICharacter
    {
        public Guid Id { get; set ; }
        public string Name { get; set ; }
        public string Image { get; set; }
        public Role Role { get; set; }
        public Gimmick Gimmick { get; set; }
        public Hailing Hailing { get; set; }
        public Entrance Entrance { get; set; }
        public List<Question> Questions { get; set; } 
        public List<Want> Wants { get; set; }
        public List<Move> Moves { get; set; }
        public List<Stat> Stats { get; set; }
        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }
        public string CharacterSheet { get; set; }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment() {
                Id = Id,
                Name = Name,
                Image = Image,
                Details = $"{Gimmick.Name} | {Role.Name}",
                CharacterSheet = CharacterSheet
            };
        }

        public byte[] BuildCharacterSheet()
        {
            // Strip "The " prefix from gimmick name to match PDF filename
            var gimmickKey = Gimmick.Name.StartsWith("The ")
                ? Gimmick.Name[4..]
                : Gimmick.Name;

            var dict = new Dictionary<string, string>
            {
                { "Name", Name ?? "" },
                { "Body", Stats?.FirstOrDefault(s => s.Name == "Body")?.Value.ToString() ?? "" },
                { "Look", Stats?.FirstOrDefault(s => s.Name == "Look")?.Value.ToString() ?? "" },
                { "Real", Stats?.FirstOrDefault(s => s.Name == "Real")?.Value.ToString() ?? "" },
                { "Work", Stats?.FirstOrDefault(s => s.Name == "Work")?.Value.ToString() ?? "" },
            };

            // Mark selected wants (each want is a named radio button child)
            if (Wants != null)
            {
                foreach (var want in Wants)
                    dict[want.Name] = "Yes";
            }

            // Check selected hailing
            if (Hailing != null)
                dict[Hailing.Name] = "Yes";

            // Check selected entrance
            if (Entrance != null)
                dict[Entrance.Name] = "Yes";

            // Fill heat questions with the player's answer (stored in Description)
            if (Questions != null)
            {
                foreach (var question in Questions)
                    dict[question.Name] = question.Description ?? "";
            }

            // Check selected moves
            if (Moves != null)
            {
                foreach (var move in Moves)
                    dict[move.Name] = "Yes";
            }

            var pdfPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                + $"/Resources/WorldWideWrestling_Character_Sheet_{gimmickKey}.pdf";

            return PDFSchema.Generate(dict, pdfPath);
        }
    }
}