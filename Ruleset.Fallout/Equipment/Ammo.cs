using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class Ammo : IEquipment
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("cost")]
        public int Cost { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity { get; set; }

        [JsonProperty("rarity")]
        public int Rarity { get; set; }
    }
}
