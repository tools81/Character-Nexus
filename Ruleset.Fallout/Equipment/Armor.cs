using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class Armor : IEquipment
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("resistancephysical")]
        public int ResistancePhysical { get; set; }

        [JsonProperty("resistanceenergy")]
        public int ResistanceEnergy { get; set; }

        [JsonProperty("resistanceradiation")]
        public int ResistanceRadiation { get; set; }

        [JsonProperty("locations")]
        public List<string> Locations { get; set; } = [];

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("cost")]
        public int Cost { get; set; }

        [JsonProperty("rarity")]
        public int Rarity { get; set; }

        [JsonProperty("modsets")]
        public List<string> ModSets { get; set; } = [];

        public List<string> Effects { get; set; } = [];
        public List<ArmorMod> InstalledMods { get; set; } = [];
    }
}
