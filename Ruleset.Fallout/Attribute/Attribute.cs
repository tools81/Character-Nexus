using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class Attribute : IAttribute
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("max")]
        public int Max { get; set; }
    }
}
