using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class Perk : IFeature
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("prerequisites")]
        public List<Prerequisite> Prerequisites { get; set; } = [];

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("rankadjustment")]
        public int RankAdjustment { get; set; }

        [JsonProperty("max")]
        public int Max { get; set; }
    }
}
