using Newtonsoft.Json;

namespace Fallout
{
    public class WeaponQuality
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
