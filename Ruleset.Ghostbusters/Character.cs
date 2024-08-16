namespace Ghostbusters
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Personality { get; set; }
        public string Notes { get; set; }
        public string Residence { get; set; }
        public string Telex { get; set; }
        public string Phone { get; set; }
        public List<Trait> Traits { get; set; }
        public List<Talent> Talents { get; set; }
        public Goal Goal { get; set; }
        public int Brownie_Points { get; set; }
        public List<Equipment> Equipments { get; set; }
        public CharacterSegment CharacterSegment => GetCharacterSegment();

        private CharacterSegment GetCharacterSegment()
        {
            return new CharacterSegment()
            {
                Id = Id,
                Name = Name,
                Image = Image,
                Level = Brownie_Points,
                Details = $"{Residence} | {Telex} | {Phone}"
            };
        }
    }
}
