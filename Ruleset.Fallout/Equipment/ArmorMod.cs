using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class ArmorMod
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("resistancephysical")]
        public int ResistancePhysical { get; set; }

        [JsonProperty("resistanceenergy")]
        public int ResistanceEnergy { get; set; }

        [JsonProperty("resistanceradiation")]
        public int ResistanceRadiation { get; set; }

        [JsonProperty("effects")]
        public List<string> Effects { get; set; } = [];

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("cost")]
        public int Cost { get; set; }

        [JsonProperty("set")]
        public string Set { get; set; }

        [JsonProperty("locations")]
        public List<string> Locations { get; set; } = [];

        [JsonProperty("skill")]
        public string? Skill { get; set; }

        [JsonProperty("prerequisites")]
        public List<Prerequisite>? Prerequisites { get; set; }
    }
}
