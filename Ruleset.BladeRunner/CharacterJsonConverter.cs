using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace BladeRunner
{
    public class CharacterJsonConverter : JsonConverter<Character>
    {
        TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

        public override Character ReadJson(JsonReader reader, Type objectType, Character existingValue, bool hasExistingValue, JsonSerializer serializer)
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
                        case "health":
                            character.Health = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "resolve":
                            character.Resolve = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "chinyen":
                            character.Chinyen = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "promotionPoints":
                            character.PromotionPoints = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "humanityPoints":
                            character.HumanityPoints = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "notes":
                            character.Notes = reader.Value.ToString();
                            break;
                        case "favoredGear":
                            character.FavoredGear = reader.Value.ToString();
                            break;
                        case "signatureItem":
                            character.SignatureItem = reader.Value.ToString();
                            break;
                        case "home":
                            character.Home = reader.Value.ToString();
                            break;
                        case "origin":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.BladeRunner.Json.Origins.json"))
                            {
                                using (var originsReader = new StreamReader(stream))
                                {
                                    var jsonContent = originsReader.ReadToEnd();
                                    var origins = JsonTo.List<Origin>(jsonContent);

                                    var origin = origins.Find(o => o.Name.ToLower == reader.Value.ToString().ToLower());

                                    character.Origin = origin;
                                }
                            }
                            break;
                        case "archetype":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Archetype.Json.Archetypes.json"))
                            {
                                using (var archetypesReader = new StreamReader(stream))
                                {
                                    var jsonContent = archetypesReader.ReadToEnd();
                                    var archetypes = JsonTo.List<Archetype>(jsonContent);

                                    var archetype = archetypes.Find(o => o.Name.ToLower() == reader.Value.ToString().ToLower());

                                    character.Archetype = archetype;
                                }
                            }
                            break;
                        case "attributes":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for attributes");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.BladeRunner.Json.Attributes.json"))
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
                        case "tenure":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.BladeRunner.Json.Tenures.json"))
                            {
                                using (var tenuresReader = new StreamReader(stream))
                                {
                                    var jsonContent = tenuresReader.ReadToEnd();
                                    var tenures = JsonTo.List<Tenure>(jsonContent);

                                    var tenure = tenures.Find(o => o.Name.ToLower() == reader.Value.ToString().ToLower());

                                    character.Tenure = tenure;
                                }
                            }
                            break;
                        case "skills":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for skills");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.BladeRunner.Json.Skills.json"))
                            {
                                using (var skillsReader = new StreamReader(stream))
                                {
                                    var jsonContent = skillsReader.ReadToEnd();
                                    var skills = JsonTo.List<Skill>(jsonContent);

                                    character.Skills = new List<Skill>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                    {
                                        var propName = reader.Value.ToString();
                                        reader.Read();
                                        var value = int.Parse(reader.Value.ToString());

                                        var found = skills.Find(p => p.Name.ToLower() == propName.ToLower());
                                        found.Value = value;
                                        character.Skills.Add(found);
                                    }
                                }
                            }
                            break;
                        case "specialties":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for specialties");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.BladeRunner.Json.Specialties.json"))
                            {
                                using (var specialtiesReader = new StreamReader(stream))
                                {
                                    var jsonContent = specialtiesReader.ReadToEnd();
                                    var specialties = JsonTo.List<Specialty>(jsonContent);

                                    character.Specialties = new List<Specialty>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        var found = specialties.Find(t => t.Name.ToLower() == reader.Value.ToString().ToLower());
                                        character.Specialties.Add(found);
                                    }
                                }
                            }
                            break;
                        case "memory":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for memory");
                            }

                            character.Memory = (Memory)reader.Value;
                            break;
                        case "relationship":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for relationship");
                            }

                            character.Relationship = (Relationship)reader.Value;
                            break;
                        case "gears":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for gears");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.BladeRunner.Json.Gears.json"))
                            {
                                using (var gearsReader = new StreamReader(stream))
                                {
                                    var jsonContent = gearsReader.ReadToEnd();
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
                        case "armors":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for armors");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.BladeRunner.Json.Armors.json"))
                            {
                                using (var armorsReader = new StreamReader(stream))
                                {
                                    var jsonContent = armorsReader.ReadToEnd();
                                    var armors = JsonTo.List<Armor>(jsonContent);

                                    character.Armors = new List<Armor>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        var found = armors.Find(t => t.Name.ToLower() == reader.Value.ToString().ToLower());
                                        character.Armors.Add(found);
                                    }
                                }
                            }
                            break;
                        case "weapons":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for weapons");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.BladeRunner.Json.Weapons.json"))
                            {
                                using (var weaponsReader = new StreamReader(stream))
                                {
                                    var jsonContent = weaponsReader.ReadToEnd();
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
                        case "vehicles":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for vehicles");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.BladeRunner.Json.Vehicles.json"))
                            {
                                using (var vehiclesReader = new StreamReader(stream))
                                {
                                    var jsonContent = vehiclesReader.ReadToEnd();
                                    var vehicles = JsonTo.List<Vehicle>(jsonContent);

                                    character.Vehicles = new List<Vehicle>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        var found = vehicles.Find(t => t.Name.ToLower() == reader.Value.ToString().ToLower());
                                        character.Vehicles.Add(found);
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            reader.Close();
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

            writer.WritePropertyName("health");
            writer.WriteValue(character.Health);

            writer.WritePropertyName("resolve");
            writer.WriteValue(character.Resolve);

            writer.WritePropertyName("chinyen");
            writer.WriteValue(character.Chinyen);

            writer.WritePropertyName("promotionPoints");
            writer.WriteValue(character.PromotionPoints);

            writer.WritePropertyName("humanityPoints");
            writer.WriteValue(character.HumanityPoints);

            writer.WritePropertyName("notes");
            writer.WriteValue(character.Notes);

            writer.WritePropertyName("favoredGear");
            writer.WriteValue(character.FavoredGear);

            writer.WritePropertyName("signatureItem");
            writer.WriteValue(character.SignatureItem);

            writer.WritePropertyName("home");
            writer.WriteValue(character.Home);

            writer.WritePropertyName("origin");
            writer.WriteStartObject();
            writer.WriteValue(character.Origin.Name);
            writer.WriteEndObject();

            writer.WritePropertyName("archetype");
            writer.WriteStartObject();
            writer.WriteValue(character.Archetype.Name);
            writer.WriteEndObject();

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

            writer.WritePropertyName("tenure");
            writer.WriteStartObject();
            writer.WriteValue(character.Tenure.Name);
            writer.WriteEndObject();

            writer.WritePropertyName("skills");
            writer.WriteStartArray();
            foreach (var skill in character.Skills)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(skill.Name);
                writer.WriteValue(skill.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("specialties");
            writer.WriteStartArray();
            foreach (var specialty in character.Specialties)
            {
                writer.WriteStartObject();
                writer.WriteValue(specialty.Name);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("memory");
            writer.WriteStartObject();
            writer.WriteValue(character.Memory);
            writer.WriteEndObject();

            writer.WritePropertyName("relationship");
            writer.WriteStartObject();
            writer.WriteValue(character.Relationship);
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

            writer.WritePropertyName("armors");
            writer.WriteStartArray();
            foreach (var armor in character.Armors)
            {
                writer.WriteStartObject();
                writer.WriteValue(armor.Name);
                writer.WriteEndObject();
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

            writer.WritePropertyName("vehicles");
            writer.WriteStartArray();
            foreach (var vehicle in character.Vehicles)
            {
                writer.WriteStartObject();
                writer.WriteValue(vehicle.Name);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
            writer.Close();
        }
    }
}
