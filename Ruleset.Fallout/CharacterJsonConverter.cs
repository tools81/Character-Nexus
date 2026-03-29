using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Fallout
{
    internal partial class CharacterJsonConverter : JsonConverter<Character>
    {
        TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

        private static readonly Regex _camelCaseSplitter = new(@"(?<=[a-z])([A-Z])", RegexOptions.Compiled);

        public override Character ReadJson(JsonReader reader, Type typeToConvert, Character? existing, bool hasExistingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);

            var character = new Character
            {
                Name = jo["name"]?.ToString() ?? "Unknown"
            };

            // Id
            var idStr = jo["id"]?.ToString();
            character.Id = Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty;

            // Scalar fields (some arrive as strings in the saved JSON)
            character.HealthPoints     = ParseInt(jo["healthPoints"]);
            character.CarryWeight      = ParseInt(jo["carryWeight"]);
            character.MeleeDamage      = ParseInt(jo["meleeDamage"]);
            character.Initiative       = ParseInt(jo["initiative"]);
            character.DamageResistance = ParseInt(jo["damageResistance"]);
            character.Defense          = ParseInt(jo["defense"]);
            character.Level            = ParseInt(jo["level"]);
            character.Caps             = ParseInt(jo["caps"]);

            // Image may arrive as an object (file-input state) rather than a plain string
            var imageToken = jo["image"];
            character.Image = imageToken?.Type == JTokenType.String ? imageToken.ToString() : string.Empty;

            // Origin / Pack — stored as plain name strings on the form
            character.Origin = new Origin { Name = jo["origin"]?.ToString() ?? string.Empty };
            character.Pack   = new Pack   { Name = jo["pack"]?.ToString()   ?? string.Empty };

            // Attributes — object whose keys are attribute names, values are int-strings
            character.Attributes = [];
            if (jo["attributes"] is JObject attributesObj)
            {
                foreach (var prop in attributesObj.Properties())
                {
                    character.Attributes.Add(new Attribute
                    {
                        Name  = prop.Name,
                        Value = ParseInt(prop.Value)
                    });
                }
            }

            // Skills — keys are camelCase with no spaces (e.g. "BigGuns" → "Big Guns")
            character.Skills = [];
            if (jo["skills"] is JObject skillsObj)
            {
                foreach (var prop in skillsObj.Properties())
                {
                    character.Skills.Add(new Skill
                    {
                        Name  = SplitCamelCase(prop.Name),
                        Value = ParseInt(prop.Value)
                    });
                }
            }

            // Traits — array of { value: "name" }
            character.Traits = [];
            if (jo["traits"] is JArray traitsArr)
            {
                foreach (var item in traitsArr)
                {
                    var val = item["value"]?.ToString();
                    if (!string.IsNullOrEmpty(val))
                        character.Traits.Add(new Trait { Name = val });
                }
            }

            // Perks — array of { value: "name", quantity: int? }
            character.Perks = [];
            if (jo["perks"] is JArray perksArr)
            {
                foreach (var item in perksArr)
                {
                    var val = item["value"]?.ToString();
                    if (!string.IsNullOrEmpty(val))
                        character.Perks.Add(new Perk
                        {
                            Name = val,
                            Rank = ParseInt(item["quantity"])
                        });
                }
            }

            // Weapons — array of weapon state objects; qualities/effects may be string or array
            character.Weapons = [];
            if (jo["weapons"] is JArray weaponsArr)
            {
                foreach (var item in weaponsArr)
                {
                    var weapon = new Weapon
                    {
                        Name      = item["value"]?.ToString() ?? string.Empty,
                        Damage    = ParseInt(item["damage"]),
                        DamageType = item["damageType"]?.ToString() ?? string.Empty,
                        FireRate  = ParseInt(item["fireRate"]),
                        Range     = item["range"]?.ToString() ?? string.Empty,
                        Weight    = ParseInt(item["weight"])
                    };

                    var qualities = item["qualities"];
                    if (qualities != null && qualities.Type != JTokenType.Null)
                        weapon.Qualities = qualities.Type == JTokenType.Array
                            ? qualities.ToObject<List<string>>() ?? []
                            : [qualities.ToString()];

                    var effects = item["effects"];
                    if (effects != null && effects.Type != JTokenType.Null)
                        weapon.Effects = effects.Type == JTokenType.Array
                            ? effects.ToObject<List<string>>() ?? []
                            : [effects.ToString()];

                    character.Weapons.Add(weapon);
                }
            }

            // Armors — array of armor state objects; locations may be string or array
            character.Armors = [];
            if (jo["armors"] is JArray armorsArr)
            {
                foreach (var item in armorsArr)
                {
                    var armor = new Armor
                    {
                        Name                 = item["value"]?.ToString() ?? string.Empty,
                        ResistancePhysical   = ParseInt(item["resistancePhysical"]),
                        ResistanceEnergy     = ParseInt(item["resistanceEnergy"]),
                        ResistanceRadiation  = ParseInt(item["resistanceRadiation"]),
                        Weight               = ParseInt(item["weight"])
                    };

                    var locations = item["locations"];
                    if (locations != null && locations.Type != JTokenType.Null)
                        armor.Locations = locations.Type == JTokenType.Array
                            ? locations.ToObject<List<string>>() ?? []
                            : [locations.ToString()];

                    character.Armors.Add(armor);
                }
            }

            // Robot Armors — array of { value: "name" }
            character.RobotArmors = [];
            if (jo["robotArmors"] is JArray robotArmorsArr)
            {
                foreach (var item in robotArmorsArr)
                {
                    character.RobotArmors.Add(new ArmorRobot
                    {
                        Name = item["value"]?.ToString() ?? string.Empty
                    });
                }
            }

            // Clothings — array of { value: "name" }
            character.Clothings = [];
            if (jo["clothings"] is JArray clothingsArr)
            {
                foreach (var item in clothingsArr)
                {
                    character.Clothings.Add(new Clothing
                    {
                        Name = item["value"]?.ToString() ?? string.Empty
                    });
                }
            }

            // Consumeables — array of { value: "name", quantity: int? }
            character.Consumeables = [];
            if (jo["consumeables"] is JArray consumeablesArr)
            {
                foreach (var item in consumeablesArr)
                {
                    character.Consumeables.Add(new Consumeable
                    {
                        Name     = item["value"]?.ToString() ?? string.Empty,
                        Quantity = item["quantity"]?.Type == JTokenType.Null ? null : ParseInt(item["quantity"])
                    });
                }
            }

            // Items — array of { value: "name", quantity: int? }
            character.Items = [];
            if (jo["items"] is JArray itemsArr)
            {
                foreach (var item in itemsArr)
                {
                    character.Items.Add(new Item
                    {
                        Name     = item["value"]?.ToString() ?? string.Empty,
                        Quantity = item["quantity"]?.Type == JTokenType.Null ? null : ParseInt(item["quantity"])
                    });
                }
            }

            // Ammos — array of { value: "name", quantity: int? }
            character.Ammos = [];
            if (jo["ammos"] is JArray ammosArr)
            {
                foreach (var item in ammosArr)
                {
                    character.Ammos.Add(new Ammo
                    {
                        Name     = item["value"]?.ToString() ?? string.Empty,
                        Quantity = item["quantity"]?.Type == JTokenType.Null ? null : ParseInt(item["quantity"])
                    });
                }
            }

            return character;
        }

        public override void WriteJson(JsonWriter writer, Character? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private static int ParseInt(JToken? token)
        {
            if (token == null || token.Type == JTokenType.Null) return 0;
            if (token.Type == JTokenType.Integer) return token.Value<int>();
            return int.TryParse(token.ToString(), out var result) ? result : 0;
        }

        // Inserts a space before each uppercase letter that follows a lowercase letter,
        // converting camelCase keys like "BigGuns" to "Big Guns".
        private static string SplitCamelCase(string input) =>
            _camelCaseSplitter.Replace(input, " $1");
    }
}
