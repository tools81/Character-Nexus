using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class RobotMod
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("effects")]
        public List<string> Effects { get; set; } = [];

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("cost")]
        public int Cost { get; set; }

        [JsonProperty("rarity")]
        public int Rarity { get; set; }

        [JsonProperty("prerequisites")]
        public List<Prerequisite>? Prerequisites { get; set; }
    }
}
