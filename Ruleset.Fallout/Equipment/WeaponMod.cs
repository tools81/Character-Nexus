using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class WeaponMod
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("weapontype")]
        public string WeaponType { get; set; }

        [JsonProperty("slot")]
        public string Slot { get; set; }

        [JsonProperty("bonusadjustments")]
        public List<BonusAdjustment> BonusAdjustments { get; set; } = [];

        [JsonProperty("cost")]
        public int Cost { get; set; }

        [JsonProperty("prerequisites")]
        public List<Prerequisite> Prerequisites { get; set; } = [];
    }
}
