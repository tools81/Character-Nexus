using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class Skill : ISkill
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("attribute")]
        public string Attribute { get; set; }

        [JsonProperty("tagged")]
        public bool Tagged { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("max")]
        public int Max { get; set; }

        [JsonProperty("clothings")]
        public List<string>? Clothings { get; set; }

        [JsonProperty("items")]
        public List<string>? Items { get; set; }

        [JsonProperty("ammos")]
        public List<string>? Ammos { get; set; }

        [JsonProperty("caps")]
        public int? Caps { get; set; }

        [JsonProperty("weapons")]
        public List<string>? Weapons { get; set; }

        [JsonProperty("userchoices")]
        public List<UserChoice>? UserChoices { get; set; }
    }
}
