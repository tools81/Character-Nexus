using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public class WeaponRobot : IEquipment
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("weapontype")]
        public string WeaponType { get; set; }

        [JsonProperty("damage")]
        public int Damage { get; set; }

        [JsonProperty("effects")]
        public List<string> Effects { get; set; } = [];

        [JsonProperty("damagetype")]
        public string DamageType { get; set; }

        [JsonProperty("firerate")]
        public int FireRate { get; set; }

        [JsonProperty("range")]
        public string Range { get; set; }

        [JsonProperty("qualities")]
        public List<string> Qualities { get; set; } = [];

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("cost")]
        public int Cost { get; set; }

        [JsonProperty("rarity")]
        public int Rarity { get; set; }

        [JsonProperty("modsets")]
        public List<string> ModSets { get; set; } = [];

        [JsonProperty("ammunition")]
        public string? Ammunition { get; set; }

        [JsonProperty("installedmods")]
        public List<WeaponMod> InstalledMods { get; set; } = [];
    }
}
