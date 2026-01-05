using System.Reflection;
using Newtonsoft.Json;
using Utility;
using System.Globalization;

namespace Marvel
{
    public class CharacterJsonConverter : JsonConverter<Character>
    {
        TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;
        
        public override Character ReadJson(JsonReader reader, Type typeToConvert, Character? existing, bool hasExistingValue, JsonSerializer serializer)
        {
            var character = new Character() { Name = "" };
            int n = 0;

            if (reader.TokenType == JsonToken.Null)
            {
                throw new JsonException("Expected StartObject token");
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = reader.Value.ToString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "id":
                            var id = reader.Value.ToString();
                            character.Id = id == string.Empty ? Guid.NewGuid() : new Guid(id);
                            break;
                        case "image":
                            character.Image = reader.Value.ToString();
                            break;
                        case "name":
                            character.Name = reader.Value.ToString();
                            break;
                        case "realName":
                            character.RealName = reader.Value.ToString();
                            break;
                        case "rank":
                            character.Rank = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "height":
                            character.Height = reader.Value.ToString();
                            break;
                        case "weight":
                            character.Weight = reader.Value.ToString();
                            break;
                        case "gender":
                            character.Gender = reader.Value.ToString();
                            break;
                        case "eyes":
                            character.Eyes = reader.Value.ToString();
                            break;
                        case "hair":
                            character.Hair = reader.Value.ToString();
                            break;
                        case "size":
                            character.Size = reader.Value.ToString();
                            break;
                        case "distinguishingFeatures":
                            character.DistinguishingFeatures = reader.Value.ToString();
                            break;
                        case "teams":
                            character.Teams = reader.Value.ToString();
                            break;
                        case "base":
                            character.Base = reader.Value.ToString();
                            break;
                        case "notes":
                            character.Notes = reader.Value.ToString();
                            break;
                        case "history":
                            character.History = reader.Value.ToString();
                            break;
                        case "personality":
                            character.Personality = reader.Value.ToString();
                            break;
                        case "health":
                            character.Health = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "focus":
                            character.Focus = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "healthDamageReduction":
                            character.HealthDamageReduction = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "focusDamageReduction":
                            character.FocusDamageReduction = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "run":
                            character.Run = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "climb":
                            character.Climb = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "swim":
                            character.Swim = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "karma":
                            character.Karma = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "initiativeModifier":
                            character.InitiativeModifier = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "attributes":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for attributes");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Marvel.Json.Attributes.json"))
                            {
                                using (var attributesReader = new StreamReader(stream))
                                {
                                    var jsonContent = attributesReader.ReadToEnd();
                                    var attributes = JsonTo.List<Attribute>(jsonContent);

                                    character.Attributes = new List<Attribute>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                    {
                                        var propName = reader.Value.ToString();
                                        reader.Read();
                                        var value = int.Parse(reader.Value.ToString());

                                        var found = attributes.Find(p => p.Name.ToLower() == propName.ToLower());
                                        found.Value = value;
                                        character.Attributes.Add(found);
                                    }
                                }
                            }
                            break;
                        case "occupation":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Marvel.Json.Occupations.json"))
                            {
                                using (var occupationsReader = new StreamReader(stream))
                                {
                                    var jsonContent = occupationsReader.ReadToEnd();
                                    var occupations = JsonTo.List<Occupation>(jsonContent);

                                    //Select input items return an object, so must read to get beyond StartObject property and into Value property
                                    reader.Read();
                                    reader.Read();

                                    var occupation = occupations.Find(o => o.Name.ToLower() == reader.Value.ToString().ToLower());

                                    //Read the end object
                                    reader.Read();

                                    character.Occupation = occupation;
                                }
                            }
                            break;
                        case "origin":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Marvel.Json.Origins.json"))
                            {
                                using (var originsReader = new StreamReader(stream))
                                {
                                    var jsonContent = originsReader.ReadToEnd();
                                    var origins = JsonTo.List<Origin>(jsonContent);

                                    reader.Read();
                                    reader.Read();

                                    var origin = origins.Find(o => o.Name.ToLower() == reader.Value.ToString().ToLower());

                                    reader.Read();

                                    character.Origin = origin;
                                }
                            }
                            break;
                        case "tag":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for tags");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Marvel.Json.Tags.json"))
                            {
                                using (var tagsReader = new StreamReader(stream))
                                {
                                    var jsonContent = tagsReader.ReadToEnd();
                                    var tags = JsonTo.List<Tag>(jsonContent);

                                    character.Tags = new List<Tag>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        reader.Read();
                                        reader.Read();
                                        
                                        var found = tags.Find(t => t.Name.ToLower() == (reader.Value.ToString()).ToLower());

                                        reader.Read();

                                        character.Tags.Add(found);
                                    }
                                }
                            }
                            break;
                        case "trait":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for traits");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Marvel.Json.Traits.json"))
                            {
                                using (var traitsReader = new StreamReader(stream))
                                {
                                    var jsonContent = traitsReader.ReadToEnd();
                                    var traits = JsonTo.List<Trait>(jsonContent);

                                    character.Traits = new List<Trait>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        reader.Read();
                                        reader.Read();

                                        var found = traits.Find(t => t.Name.ToLower() == (reader.Value.ToString()).ToLower());

                                        reader.Read();

                                        character.Traits.Add(found);
                                    }
                                }
                            }
                            break;
                        case "powers":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for powers");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Marvel.Json.Powers.json"))
                            {
                                using (var powersReader = new StreamReader(stream))
                                {
                                    var jsonContent = powersReader.ReadToEnd();
                                    var powers = JsonTo.List<Power>(jsonContent);

                                    character.Powers = new List<Power>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                    {
                                        var propName = reader.Value.ToString();
                                        reader.Read();
                                        var value = (bool)reader.Value;

                                        if (value)
                                        {
                                            var found = powers.Find(p => p.Name.ToLower() == propName.ToLower());
                                            character.Powers.Add(found);
                                        }
                                    }
                                }
                            }
                            break;
                        case "weapon":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for weapons");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Marvel.Json.Weapons.json"))
                            {
                                using (var weaponsReader = new StreamReader(stream))
                                {
                                    var jsonContent = weaponsReader.ReadToEnd();
                                    var weapons = JsonTo.List<Weapon>(jsonContent);

                                    character.Weapons = new List<Weapon>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        reader.Read();
                                        reader.Read();

                                        var found = weapons.Find(w => w.Name.ToLower() == (reader.Value.ToString()).ToLower());

                                        reader.Read();

                                        character.Weapons.Add(found);
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            return character;
        }

        public override void WriteJson(JsonWriter writer, Character character, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("id");
            writer.WriteValue(character.Id);

            writer.WritePropertyName("image");
            writer.WriteValue(character.Image);

            writer.WritePropertyName("name");
            writer.WriteValue(character.Name);

            writer.WritePropertyName("realName");
            writer.WriteValue(character.RealName);

            writer.WritePropertyName("rank");
            writer.WriteValue(character.Rank);

            writer.WritePropertyName("height");
            writer.WriteValue(character.Height);

            writer.WritePropertyName("weight");
            writer.WriteValue(character.Weight);

            writer.WritePropertyName("gender");
            writer.WriteValue(character.Gender);

            writer.WritePropertyName("eyes");
            writer.WriteValue(character.Eyes);

            writer.WritePropertyName("hair");
            writer.WriteValue(character.Hair);

            writer.WritePropertyName("size");
            writer.WriteValue(character.Size);

            writer.WritePropertyName("distinguishingFeatures");
            writer.WriteValue(character.DistinguishingFeatures);

            writer.WritePropertyName("teams");
            writer.WriteValue(character.Teams);

            writer.WritePropertyName("base");
            writer.WriteValue(character.Base);

            writer.WritePropertyName("notes");
            writer.WriteValue(character.Notes);

            writer.WritePropertyName("history");
            writer.WriteValue(character.History);

            writer.WritePropertyName("personality");
            writer.WriteValue(character.Personality);

            writer.WritePropertyName("attributes");
            writer.WriteStartArray();
            foreach (var attribute in character.Attributes)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(attribute.Name);
                writer.WriteValue(attribute.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("occupation");
            writer.WriteStartObject();
            writer.WriteValue(character.Occupation.Name);
            writer.WriteEndObject();

            writer.WritePropertyName("origin");
            writer.WriteStartObject();
            writer.WriteValue(character.Origin.Name);
            writer.WriteEndObject();

            writer.WritePropertyName("tags");
            writer.WriteStartArray();
            foreach (var tag in character.Tags)
            {
                writer.WriteStartObject();
                writer.WriteValue(tag.Name);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("traits");
            writer.WriteStartArray();
            foreach (var trait in character.Traits)
            {
                writer.WriteStartObject();
                writer.WriteValue(trait.Name);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("powers");
            writer.WriteStartArray();
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Marvel.Json.Powers.json"))
            {
                using (var powersReader = new StreamReader(stream))
                {
                    var jsonContent = powersReader.ReadToEnd();
                    var powers = JsonTo.List<Power>(jsonContent);

                    foreach (var power in powers)
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName(power.Name);
                        writer.WriteValue(character.Powers.Contains(power));
                        writer.WriteEndObject();
                    }
                }
            }
            writer.WriteEndArray();

            writer.WritePropertyName("weapons");
            writer.WriteStartArray();
            foreach (var weapon in character.Weapons)
            {
                writer.WriteStartObject();
                writer.WriteValue(weapon.Name);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}
