using System.Reflection;
using Newtonsoft.Json;
using Utility;
using System.Globalization;
using System;
using System.IO;
using System.Collections.Generic;

namespace DarkCrystal
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
                        case "notes":
                            character.Notes = reader.Value.ToString();
                            break;
                        case "gender":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.DarkCrystal.Json.Gender.json"))
                            {
                                using (var gendersReader = new StreamReader(stream))
                                {
                                    var jsonContent = gendersReader.ReadToEnd();
                                    var genders = JsonTo.List<Gender>(jsonContent);

                                    reader.Read();
                                    reader.Read();

                                    var gender = genders.Find(o => o.Name.ToLower() == reader.Value.ToString().ToLower());

                                    reader.Read();

                                    character.Gender = gender;
                                }
                            }
                            break;
                        case "clan":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for clans");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Marvel.Json.Clans.json"))
                            {
                                using (var clansReader = new StreamReader(stream))
                                {
                                    var jsonContent = clansReader.ReadToEnd();
                                    var clans = JsonTo.List<Clan>(jsonContent);

                                    character.Clan = new Clan();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                    {
                                        var propName = reader.Value.ToString();
                                        reader.Read();
                                        var value = (bool)reader.Value;

                                        if (value)
                                        {
                                            var found = clans.Find(p => p.Name.ToLower() == propName.ToLower());
                                            character.Clan = found;
                                        }
                                    }
                                }
                            }
                            break;
                        case "traits":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for traits");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.DarkCrystal.Json.Traits.json"))
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

                                        var found = traits.Find(w => w.Name.ToLower() == reader.Value.ToString().ToLower());

                                        reader.Read();

                                        character.Traits.Add(found);
                                    }
                                }
                            }
                            break;
                        case "skills":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for powers");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.DarkCrystal.Json.Skills.json"))
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
                                        var value = (bool)reader.Value;

                                        if (value)
                                        {
                                            var found = skills.Find(p => p.Name.ToLower() == propName.ToLower());
                                            found.Trained = true;
                                            character.Skills.Add(found);
                                        }
                                        else
                                        {
                                            var found = skills.Find(p => p.Name.ToLower() == propName.ToLower());
                                            found.Trained = false;
                                            character.Skills.Add(found);
                                        }
                                    }
                                }
                            }
                            break;
                        case "specializations":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for specializations");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.DarkCrystal.Json.Specializations.json"))
                            {
                                using (var specializationsReader = new StreamReader(stream))
                                {
                                    var jsonContent = specializationsReader.ReadToEnd();
                                    var specializations = JsonTo.List<Specialization>(jsonContent);

                                    character.Specializations = new List<Specialization>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                    {
                                        var propName = reader.Value.ToString();
                                        reader.Read();
                                        var value = int.Parse(reader.Value.ToString());

                                        var found = specializations.Find(p => p.Name.ToLower() == propName.ToLower());
                                        found.Master = true;
                                        character.Specializations.Add(found);
                                    }
                                }
                            }
                            break;
                        case "flaws":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for flaw");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.DarkCrystal.Json.Flaws.json"))
                            {
                                using (var flawsReader = new StreamReader(stream))
                                {
                                    var jsonContent = flawsReader.ReadToEnd();
                                    var flaws = JsonTo.List<Flaw>(jsonContent);

                                    character.Flaws = new List<Flaw>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        reader.Read();
                                        reader.Read();

                                        var found = flaws.Find(w => w.Name.ToLower() == reader.Value.ToString().ToLower());

                                        reader.Read();

                                        character.Flaws.Add(found);
                                    }
                                }
                            }
                            break;
                        case "gear":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for gear");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.DarkCrystal.Json.Gear.json"))
                            {
                                using (var gearsReader = new StreamReader(stream))
                                {
                                    var jsonContent = gearsReader.ReadToEnd();
                                    var gears = JsonTo.List<Gear>(jsonContent);

                                    character.Gear = new List<Gear>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        reader.Read();
                                        reader.Read();

                                        var found = gears.Find(w => w.Name.ToLower() == (reader.Value.ToString()).ToLower());

                                        reader.Read();

                                        character.Gear.Add(found);
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            return character;
        }

        public override void WriteJson(JsonWriter writer, Character? value, JsonSerializer serializer)
        {
            var character = value as Character;

            if (character != null)
            {
                var jsonObject = new Newtonsoft.Json.Linq.JObject
                {
                    { "Id", character.Id.ToString() },
                    { "Name", character.Name },
                    { "Image", character.Image },
                    { "Skills", Newtonsoft.Json.Linq.JArray.FromObject(character.Skills, serializer) },
                    { "Clan", Newtonsoft.Json.Linq.JObject.FromObject(character.Clan, serializer) }
                };
            }
        }
    }
}