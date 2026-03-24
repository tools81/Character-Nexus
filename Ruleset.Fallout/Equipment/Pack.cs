using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class Pack : IEquipment
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("caps")]
        public int? Caps { get; set; }

        [JsonProperty("weapons")]
        public List<string>? Weapons { get; set; }

        [JsonProperty("clothings")]
        public List<string>? Clothings { get; set; }

        [JsonProperty("items")]
        public List<string>? Items { get; set; }

        [JsonProperty("armors")]
        public List<string>? Armors { get; set; }

        [JsonProperty("ammos")]
        public List<string>? Ammos { get; set; }

        [JsonProperty("mods")]
        public List<string>? Mods { get; set; }

        [JsonProperty("userchoices")]
        public List<UserChoice>? UserChoices { get; set; }
    }
}
