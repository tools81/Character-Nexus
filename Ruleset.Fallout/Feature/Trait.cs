using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class Trait : IFeature
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("userchoices")]
        public List<UserChoice>? UserChoices { get; set; }

        [JsonProperty("bonusadjustments")]
        public List<BonusAdjustment>? BonusAdjustments { get; set; }
    }
}
