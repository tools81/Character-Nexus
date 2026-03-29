using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class ArmorMod
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("set")]
        public string Set { get; set; }

        [JsonProperty("locations")]
        public List<string> Locations { get; set; } = [];

        [JsonProperty("bonusadjustments")]
        public List<BonusAdjustment> BonusAdjustments { get; set; } = [];

        [JsonProperty("bonuscharacteristics")]
        public List<BonusCharacteristic> BonusCharacteristics { get; set; } = [];

        [JsonProperty("cost")]
        public int Cost { get; set; }

        [JsonProperty("skill")]
        public string? Skill { get; set; }

        [JsonProperty("prerequisites")]
        public List<Prerequisite>? Prerequisites { get; set; }
    }
}
