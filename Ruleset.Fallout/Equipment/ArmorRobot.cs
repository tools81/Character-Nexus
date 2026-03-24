using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class ArmorRobot : IEquipment
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("resistancephysical")]
        public int ResistancePhysical { get; set; }

        [JsonProperty("resistanceenergy")]
        public int ResistanceEnergy { get; set; }

        [JsonProperty("locations")]
        public List<string> Locations { get; set; } = [];

        [JsonProperty("carry")]
        public int Carry { get; set; }

        [JsonProperty("cost")]
        public int Cost { get; set; }

        [JsonProperty("prerequisites")]
        public List<Prerequisite> Prerequisites { get; set; } = [];
    }
}
