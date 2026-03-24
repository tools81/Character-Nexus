using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class Origin : IFeature
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("userchoices")]
        public List<UserChoice> UserChoices { get; set; } = [];
    }
}
