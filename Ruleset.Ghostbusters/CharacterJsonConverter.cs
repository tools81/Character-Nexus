using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Utility;

namespace Ghostbusters
{
    public class CharacterJsonConverter : JsonConverter<Character>
    {
        TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

        public override Character ReadJson(JsonReader reader, Type objectType, Character existingValue, bool hasExistingValue, JsonSerializer serializer)
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
                            character.Id = id == string.Empty ? new Guid() : new Guid(id);
                            break;
                        case "image":
                            character.Image = reader.Value.ToString();
                            break;
                        case "name":
                            character.Name = reader.Value.ToString();
                            break;
                        case "alias":
                            character.Alias = reader.Value.ToString();
                            break;
                        case "browniePoints":
                            character.BrowniePoints = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "description":
                            character.Description = reader.Value.ToString();
                            break;
                        case "notes":
                            character.Notes = reader.Value.ToString();
                            break;
                        case "residence":
                            character.Residence = reader.Value.ToString();
                            break;
                        case "telex":
                            character.Telex = reader.Value.ToString();
                            break;
                        case "phone":
                            character.Phone = reader.Value.ToString();
                            break;
                        case "traits":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for traits");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Ghostbusters.Json.Traits.json"))
                            {
                                using (var traitsReader = new StreamReader(stream))
                                {
                                    var jsonContent = traitsReader.ReadToEnd();
                                    var traits = JsonTo.List<Trait>(jsonContent);

                                    character.Traits = new List<Trait>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                    {
                                        var propName = reader.Value.ToString();
                                        reader.Read();
                                        var value = int.Parse(reader.Value.ToString());

                                        var found = traits.Find(p => p.Name.ToLower() == propName.ToLower());
                                        found.Value = value;
                                        character.Traits.Add(found);
                                    }
                                }
                            }
                            break;
                        case "talents":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for powers");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Ghostbusters.Json.Talents.json"))
                            {
                                using (var talentsReader = new StreamReader(stream))
                                {
                                    var jsonContent = talentsReader.ReadToEnd();
                                    var talents = JsonTo.List<Talent>(jsonContent);

                                    character.Talents = new List<Talent>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                    {
                                        var propName = reader.Value.ToString();
                                        reader.Read();
                                        var found = talents.Find(p => p.Name.ToLower() == reader.Value.ToString().ToLower());
                                        character.Talents.Add(found);
                                    }
                                }
                            }
                            break;
                        case "goal":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Ghostbusters.Json.Goals.json"))
                            {
                                using (var goalsReader = new StreamReader(stream))
                                {
                                    var jsonContent = goalsReader.ReadToEnd();
                                    var goals = JsonTo.List<Goal>(jsonContent);

                                    var goal = goals.Find(o => o.Name.ToLower() == reader.Value.ToString().ToLower());

                                    character.Goal = goal;
                                }
                            }
                            break;
                        case "gears":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for tags");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Ghostbusters.Json.Gears.json"))
                            {
                                using (var gearReader = new StreamReader(stream))
                                {
                                    var jsonContent = gearReader.ReadToEnd();
                                    var gears = JsonTo.List<Gear>(jsonContent);

                                    character.Gears = new List<Gear>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        var found = gears.Find(t => t.Name.ToLower() == reader.Value.ToString().ToLower());
                                        character.Gears.Add(found);
                                    }
                                }
                            }
                            break;
                        case "weapons":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for weapons");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Ghostbusters.Json.Weapons.json"))
                            {
                                using (var weaponReader = new StreamReader(stream))
                                {
                                    var jsonContent = weaponReader.ReadToEnd();
                                    var weapons = JsonTo.List<Weapon>(jsonContent);

                                    character.Weapons = new List<Weapon>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        var found = weapons.Find(t => t.Name.ToLower() == reader.Value.ToString().ToLower());
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

            writer.WritePropertyName("browniePoints");
            writer.WriteValue(character.BrowniePoints);

            writer.WritePropertyName("notes");
            writer.WriteValue(character.Notes);

            writer.WritePropertyName("residence");
            writer.WriteValue(character.Residence);

            writer.WritePropertyName("telex");
            writer.WriteValue(character.Telex);

            writer.WritePropertyName("phone");
            writer.WriteValue(character.Phone);

            writer.WritePropertyName("traits");
            writer.WriteStartArray();
            foreach (var trait in character.Traits)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(trait.Name);
                writer.WriteValue(trait.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("talents");
            writer.WriteStartArray();
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Ghostbusters.Json.Talents.json"))
            {
                using (var talentsReader = new StreamReader(stream))
                {
                    var jsonContent = talentsReader.ReadToEnd();
                    var talents = JsonTo.List<Talent>(jsonContent);

                    foreach (var talent in talents)
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName(talent.Name);
                        writer.WriteValue(character.Talents.Contains(talent));
                        writer.WriteEndObject();
                    }
                }
            }
            writer.WriteEndArray();

            writer.WritePropertyName("goal");
            writer.WriteStartObject();
            writer.WriteValue(character.Goal);
            writer.WriteEndObject();

            writer.WritePropertyName("gears");
            writer.WriteStartArray();
            foreach (var gear in character.Gears)
            {
                writer.WriteStartObject();
                writer.WriteValue(gear.Name);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
            writer.Close();
        }
    }
}
