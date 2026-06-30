using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace FinalFantasy
{
    internal class CharacterJsonConverter : JsonConverter<Character>
    {
        TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

        public override Character ReadJson(JsonReader reader, Type typeToConvert, Character? existing, bool hasExistingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);

            var character = new Character
            {
                Name = jo["name"]?.ToString() ?? "Unknown"
            };

            // Id
            var idStr = jo["id"]?.ToString();
            character.Id = Guid.TryParse(idStr, out var guid) ? guid : Guid.NewGuid();

            // Image may arrive as an object (file-input state) rather than a plain string
            var imageToken = jo["image"];
            character.Image = imageToken?.Type == JTokenType.String ? imageToken.ToString() : string.Empty;

            // Scalar fields
            character.Level        = ParseInt(jo["level"]);
            character.HP           = ParseInt(jo["hp"]);
            character.MP           = ParseInt(jo["mp"]);
            character.Defense      = ParseInt(jo["defense"]);
            character.MagicDefense = ParseInt(jo["magicDefense"]);
            character.Vigilance    = ParseInt(jo["vigilance"]);
            character.Speed        = ParseInt(jo["speed"]);
            character.Size         = jo["size"]?.ToString() ?? "Medium";
            character.Profile      = jo["profile"]?.ToString() ?? string.Empty;

            // Origins — stored as plain name strings
            character.Race    = new Race    { Name = jo["race"]?.ToString()    ?? string.Empty };
            character.Subrace = new Subrace { Name = jo["subrace"]?.ToString() ?? string.Empty };
            character.Role    = new Role    { Name = jo["role"]?.ToString()    ?? string.Empty };
            character.Job     = new Job     { Name = jo["job"]?.ToString()     ?? string.Empty };

            // Attributes — object whose keys are attribute names, values are ints
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

            // Abilities — array of { value: "name" }
            character.Abilities = [];
            if (jo["abilities"] is JArray abilitiesArr)
            {
                foreach (var item in abilitiesArr)
                {
                    var val = item["value"]?.ToString();
                    if (!string.IsNullOrEmpty(val))
                        character.Abilities.Add(new Ability { Name = val });
                }
            }

            // Items — array of { value: "name" }
            character.Items = [];
            if (jo["items"] is JArray itemsArr)
            {
                foreach (var item in itemsArr)
                {
                    var val = item["value"]?.ToString();
                    if (!string.IsNullOrEmpty(val))
                        character.Items.Add(new Item { Name = val });
                }
            }

            // Augmentation — single select stored as a name string
            var augName = jo["augmentation"]?.ToString();
            character.Augmentation = new Augmentation { Name = augName ?? string.Empty };

            // LimitBreak — single select stored as a name string
            var lbName = jo["limitBreak"]?.ToString();
            character.LimitBreak = new LimitBreak { Name = lbName ?? string.Empty };

            return character;
        }

        public override void WriteJson(JsonWriter writer, Character? value, JsonSerializer serializer)
        {
            if (value is not Character character)
            {
                writer.WriteNull();
                return;
            }

            var jo = new JObject
            {
                ["id"]           = character.Id.ToString(),
                ["name"]         = character.Name,
                ["image"]        = character.Image,
                ["level"]        = character.Level,
                ["hp"]           = character.HP,
                ["mp"]           = character.MP,
                ["defense"]      = character.Defense,
                ["magicDefense"] = character.MagicDefense,
                ["vigilance"]    = character.Vigilance,
                ["speed"]        = character.Speed,
                ["size"]         = character.Size,
                ["profile"]      = character.Profile,
                ["race"]         = character.Race?.Name,
                ["subrace"]      = character.Subrace?.Name,
                ["role"]         = character.Role?.Name,
                ["job"]          = character.Job?.Name,
            };

            // Attributes → { "STR": 5, "DEX": 3, ... }
            var attributesObj = new JObject();
            foreach (var attr in character.Attributes ?? [])
                attributesObj[attr.Name] = attr.Value;
            jo["attributes"] = attributesObj;

            // Traits → [{ "value": "name" }]
            var traitsArr = new JArray();
            foreach (var trait in character.Traits ?? [])
                traitsArr.Add(new JObject { ["value"] = trait.Name });
            jo["traits"] = traitsArr;

            // Abilities → [{ "value": "name" }]
            var abilitiesArr = new JArray();
            foreach (var ability in character.Abilities ?? [])
                abilitiesArr.Add(new JObject { ["value"] = ability.Name });
            jo["abilities"] = abilitiesArr;

            // Items → [{ "value": "name" }]
            var itemsArr = new JArray();
            foreach (var item in character.Items ?? [])
                itemsArr.Add(new JObject { ["value"] = item.Name });
            jo["items"] = itemsArr;

            // Augmentation and LimitBreak → plain name strings
            jo["augmentation"] = character.Augmentation?.Name;
            jo["limitBreak"]   = character.LimitBreak?.Name;

            jo.WriteTo(writer);
        }

        private static int ParseInt(JToken? token)
        {
            if (token == null || token.Type == JTokenType.Null) return 0;
            if (token.Type == JTokenType.Integer) return token.Value<int>();
            return int.TryParse(token.ToString(), out var result) ? result : 0;
        }
    }
}
