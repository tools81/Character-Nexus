using System;
using System.Collections.Generic;
using Utility;

namespace DarkCrystal
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string Image { get; set; } = string.Empty;
        public List<Skill> Skills { get; set; } = new List<Skill>();
        public Clan Clan { get; set; } = new Clan();
        public Gender Gender { get; set; } = new Gender();
        public List<Flaw> Flaw { get; set; } = new List<Flaw>();
        public List<Trait> Traits { get; set; } = new List<Trait>();
        public List<Specialization> Specializations { get; set; } = new List<Specialization>();
        public List<Gear> Gear { get; set; } = new List<Gear>();
        public string Notes { get; set; } = string.Empty;
        public CharacterSegment CharacterSegment { get => GetCharacterSegment(); }
        public string CharacterSheet { get; set; } = string.Empty;

        public byte[] BuildCharacterSheet()
        {
            throw new NotImplementedException();
        }

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                Image = Image,
                Details = $"{Clan?.Name}",
                CharacterSheet = CharacterSheet
            };
        }
    }
}