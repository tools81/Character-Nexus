using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Utility;

namespace Transformers
{
    internal class CharacterJsonConverter : JsonConverter<Character>
    {
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
                            var id = reader.Value?.ToString() ?? "";
                            character.Id = id == string.Empty ? Guid.NewGuid() : new Guid(id);
                            break;
                        case "name":
                            character.Name = reader.Value?.ToString() ?? "";
                            break;
                        case "image":
                            character.Image = reader.Value.ToString();
                            break;
                        case "description":
                            character.Description = reader.Value?.ToString() ?? "";
                            break;
                        case "notes":
                            character.Notes = reader.Value?.ToString() ?? "";
                            break;
                        case "level":
                            character.Level = int.TryParse(reader.Value?.ToString(), out n) ? n : 1;
                            break;
                        case "faction":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Transformers.Json.Factions.json"))
                            {
                                if (stream != null)
                                {
                                    using (var factionsReader = new StreamReader(stream))
                                    {
                                        var jsonContent = factionsReader.ReadToEnd();
                                        var factions = JsonConvert.DeserializeObject<List<Faction>>(jsonContent);
                                        var faction = factions?.FirstOrDefault(f => f.Name.Equals(reader.Value?.ToString(), StringComparison.OrdinalIgnoreCase));
                                        character.Faction = faction;
                                    }
                                }
                            }
                            break;
                        case "origin":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Transformers.Json.Origins.json"))
                            {
                                using (var originsReader = new StreamReader(stream))
                                {
                                    var jsonContent = originsReader.ReadToEnd();
                                    var origins = JsonConvert.DeserializeObject<List<Origin>>(jsonContent);
                                    var origin = origins?.FirstOrDefault(o => o.Name.Equals(reader.Value?.ToString(), StringComparison.OrdinalIgnoreCase));
                                    character.Origin = origin;
                                }
                            }
                            break;
                        case "role":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Transformers.Json.Roles.json"))
                            {
                                using (var rolesReader = new StreamReader(stream))
                                {
                                    var jsonContent = rolesReader.ReadToEnd();
                                    var roles = JsonConvert.DeserializeObject<List<Role>>(jsonContent);
                                    var role = roles?.FirstOrDefault(r => r.Name.Equals(reader.Value?.ToString(), StringComparison.OrdinalIgnoreCase));
                                    character.Role = role;
                                }
                            }
                            break;
                        case "focus":
                            if (reader.TokenType == JsonToken.StartObject)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Transformers.Json.Focuses.json"))
                                {
                                    using (var focusesReader = new StreamReader(stream))
                                    {
                                        var jsonContent = focusesReader.ReadToEnd();
                                        var focuses = JsonConvert.DeserializeObject<List<Focus>>(jsonContent);

                                        while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                        {
                                            if (reader.TokenType == JsonToken.PropertyName)
                                            {
                                                reader.Read();
                                                var focusName = reader.Value?.ToString();
                                                var focus = focuses?.FirstOrDefault(f => f.Name.Equals(focusName, StringComparison.OrdinalIgnoreCase));
                                                if (focus != null)
                                                {
                                                    character.Focus = focus;
                                                    reader.Read(); //Read end object token
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }                            
                            }
                            break;
                        case "essences":
                            if (reader.TokenType == JsonToken.StartObject)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Transformers.Json.Essences.json"))
                                {
                                    using (var essencesReader = new StreamReader(stream))
                                    {
                                        var jsonContent = essencesReader.ReadToEnd();
                                        var essences = JsonConvert.DeserializeObject<List<Essence>>(jsonContent);
                                        character.Essences = new List<Essence>();

                                        while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                        {
                                            if (reader.TokenType == JsonToken.PropertyName)
                                            {
                                                var essenceName = reader.Value?.ToString();
                                                reader.Read();
                                                var value = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;

                                                var essence = essences?.FirstOrDefault(e => e.Name.Equals(essenceName, StringComparison.OrdinalIgnoreCase));
                                                if (essence != null)
                                                {
                                                    essence.Value = value;
                                                    character.Essences.Add(essence);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "skills":
                            if (reader.TokenType == JsonToken.StartObject)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Transformers.Json.Skills.json"))
                                {
                                    using (var skillsReader = new StreamReader(stream))
                                    {
                                        var jsonContent = skillsReader.ReadToEnd();
                                        var skills = JsonConvert.DeserializeObject<List<Skill>>(jsonContent);
                                        character.Skills = new List<Skill>();

                                        while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                        {
                                            if (reader.TokenType == JsonToken.PropertyName)
                                            {
                                                var skillName = reader.Value?.ToString();
                                                reader.Read();
                                                var value = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;

                                                var skill = skills?.FirstOrDefault(s => s.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase));
                                                if (skill != null)
                                                {
                                                    skill.Value = value;
                                                    character.Skills.Add(skill);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "influences":
                            if (reader.TokenType == JsonToken.StartArray)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Transformers.Json.Influences.json"))
                                {
                                    using (var influencesReader = new StreamReader(stream))
                                    {
                                        var jsonContent = influencesReader.ReadToEnd();
                                        var influences = JsonConvert.DeserializeObject<List<Influence>>(jsonContent);
                                        character.Influences = new List<Influence>();

                                        while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                        {
                                            var influenceName = reader.Value?.ToString();
                                            var influence = influences?.FirstOrDefault(i => i.Name.Equals(influenceName, StringComparison.OrdinalIgnoreCase));
                                            if (influence != null)
                                            {
                                                character.Influences.Add(influence);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "perks":
                            if (reader.TokenType == JsonToken.StartArray)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Transformers.Json.Perks.json"))
                                {
                                    using (var perksReader = new StreamReader(stream))
                                    {
                                        var jsonContent = perksReader.ReadToEnd();
                                        var perks = JsonConvert.DeserializeObject<List<Perk>>(jsonContent);
                                        character.Perks = new List<Perk>();

                                        while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                        {
                                            var perkName = reader.Value?.ToString();
                                            var perk = perks?.FirstOrDefault(p => p.Name.Equals(perkName, StringComparison.OrdinalIgnoreCase));
                                            if (perk != null)
                                            {
                                                character.Perks.Add(perk);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "hangups":
                            if (reader.TokenType == JsonToken.StartArray)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Transformers.Json.Hang-Ups.json"))
                                {
                                    using (var hangUpsReader = new StreamReader(stream))
                                    {
                                        var jsonContent = hangUpsReader.ReadToEnd();
                                        var hangUps = JsonConvert.DeserializeObject<List<HangUp>>(jsonContent);
                                        character.HangUps = new List<HangUp>();

                                        while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                        {
                                            var hangUpName = reader.Value?.ToString();
                                            var hangUp = hangUps?.FirstOrDefault(h => h.Name.Equals(hangUpName, StringComparison.OrdinalIgnoreCase));
                                            if (hangUp != null)
                                            {
                                                character.HangUps.Add(hangUp);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "specialization":
                            if (reader.TokenType == JsonToken.StartObject)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Transformers.Json.Specializations.json"))
                                {
                                    using (var specializationsReader = new StreamReader(stream))
                                    {
                                        var jsonContent = specializationsReader.ReadToEnd();
                                        var specializations = JsonConvert.DeserializeObject<List<Specialization>>(jsonContent);
                                        character.Specializations = new List<Specialization>();

                                        while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                        {
                                            if (reader.TokenType == JsonToken.PropertyName)
                                            {
                                                reader.Read();
                                                if (reader.TokenType == JsonToken.StartArray)
                                                {
                                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                                    {
                                                        var specializationName = reader.Value?.ToString();
                                                        var specialization = specializations?.FirstOrDefault(s => s.Name.Equals(specializationName, StringComparison.OrdinalIgnoreCase));
                                                        if (specialization != null)
                                                        {
                                                            character.Specializations.Add(specialization);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            return character;
        }

        public override void WriteJson(JsonWriter writer, Character value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
