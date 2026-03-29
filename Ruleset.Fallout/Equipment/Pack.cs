using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class Pack : IEquipment
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("origin")]
        public string? Origin { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("bonusadjustments")]
        public List<BonusAdjustment> BonusAdjustments { get; set; } = [];

        [JsonProperty("bonuscharacteristics")]
        public List<BonusCharacteristic> BonusCharacteristics { get; set; } = [];

        [JsonProperty("mods")]
        public List<string>? Mods { get; set; }

        [JsonProperty("userchoices")]
        public List<UserChoice>? UserChoices { get; set; }
    }
}
