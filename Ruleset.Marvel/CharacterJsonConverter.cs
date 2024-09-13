using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
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
                    string propertyName = (string)reader.Value;
                    reader.Read();

                    switch (propertyName)
                    {
                        case "id":
                            var id = (string)reader.Value;
                            character.Id = id == string.Empty ? new Guid() : new Guid(id);
                            break;
                        case "image":
                            character.Image = (string)reader.Value;
                            break;
                        case "name":
                            character.Name = (string)reader.Value;
                            break;
                        case "realName":
                            character.RealName = (string)reader.Value;
                            break;
                        case "rank":
                            character.Rank = int.Parse((string)reader.Value);
                            break;
                        case "height":
                            character.Height = (string)reader.Value;
                            break;
                        case "weight":
                            character.Weight = (string)reader.Value;
                            break;
                        case "gender":
                            character.Gender = (string)reader.Value;
                            break;
                        case "eyes":
                            character.Eyes = (string)reader.Value;
                            break;
                        case "hair":
                            character.Hair = (string)reader.Value;
                            break;
                        case "size":
                            character.Size = (string)reader.Value;
                            break;
                        case "distinguishingFeatures":
                            character.DistinguishingFeatures = (string)reader.Value;
                            break;
                        case "teams":
                            character.Teams = (string)reader.Value;
                            break;
                        case "base":
                            character.Base = (string)reader.Value;
                            break;
                        case "notes":
                            character.Notes = (string)reader.Value;
                            break;
                        case "history":
                            character.History = (string)reader.Value;
                            break;
                        case "personality":
                            character.Personality = (string)reader.Value;
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
                                        var propName = (string)reader.Value;
                                        reader.Read();
                                        var value = int.Parse((string)reader.Value);

                                        var found = attributes.Find(p => p.Name == _textInfo.ToTitleCase(propName));
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

                                    var occupation = occupations.Find(o => o.Name == _textInfo.ToTitleCase((string)reader.Value));

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

                                    var origin = origins.Find(o => o.Name == _textInfo.ToTitleCase((string)reader.Value));

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
                                        var found = tags.Find(t => t.Name == _textInfo.ToTitleCase((string)reader.Value));
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
                                        var found = traits.Find(t => t.Name == _textInfo.ToTitleCase((string)reader.Value));
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
                                        var propName = (string)reader.Value;
                                        reader.Read();
                                        var value = (bool)reader.Value;

                                        if (value)
                                        {
                                            var found = powers.Find(p => p.Name == _textInfo.ToTitleCase(propName));
                                            character.Powers.Add(found);
                                        }
                                    }
                                }
                            }
                            break;
                        case "weapons":
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
                                        var found = weapons.Find(w => w.Name == _textInfo.ToTitleCase((string)reader.Value));
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
