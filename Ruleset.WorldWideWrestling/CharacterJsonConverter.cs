using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Utility;

namespace WorldWideWrestling
{
    public class CharacterJsonConverter : JsonConverter<Character>
    {
        TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

        public override Character ReadJson(JsonReader reader, Type objectType, Character existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var character = new Character() { Name = "" };
            int n = 0;

            if (reader.TokenType == JsonToken.Null)
                throw new JsonException("Expected StartObject token");

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                    break;

                if (reader.TokenType != JsonToken.PropertyName)
                    continue;

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
                        if (reader.TokenType == JsonToken.StartObject)
                            reader.Skip();
                        else
                            character.Image = reader.Value?.ToString() ?? "";
                        break;
                    case "role":
                        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.WorldWideWrestling.Json.Roles.json"))
                        using (var rolesReader = new StreamReader(stream))
                        {
                            var roles = JsonConvert.DeserializeObject<List<Role>>(rolesReader.ReadToEnd());
                            character.Role = roles?.FirstOrDefault(r => r.Name.Equals(reader.Value?.ToString(), StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    case "gimmick":
                        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.WorldWideWrestling.Json.Gimmicks.json"))
                        using (var gimmicksReader = new StreamReader(stream))
                        {
                            var gimmicks = JsonConvert.DeserializeObject<List<Gimmick>>(gimmicksReader.ReadToEnd());
                            character.Gimmick = gimmicks?.FirstOrDefault(g => g.Name.Equals(reader.Value?.ToString(), StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    case "stats":
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.WorldWideWrestling.Json.Stats.json"))
                            using (var statsReader = new StreamReader(stream))
                            {
                                var stats = JsonConvert.DeserializeObject<List<Stat>>(statsReader.ReadToEnd());
                                character.Stats = new List<Stat>();

                                while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                {
                                    if (reader.TokenType == JsonToken.PropertyName)
                                    {
                                        var statName = reader.Value?.ToString();
                                        reader.Read();
                                        var value = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;
                                        var stat = stats?.FirstOrDefault(s => s.Name.Equals(statName, StringComparison.OrdinalIgnoreCase));
                                        if (stat != null)
                                        {
                                            stat.Value = value;
                                            character.Stats.Add(stat);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "wants":
                        if (reader.TokenType == JsonToken.StartArray)
                        {
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.WorldWideWrestling.Json.Wants.json"))
                            using (var wantsReader = new StreamReader(stream))
                            {
                                var wants = JsonConvert.DeserializeObject<List<Want>>(wantsReader.ReadToEnd());
                                character.Wants = new List<Want>();

                                while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                {
                                    var want = wants?.FirstOrDefault(w => w.Name.Equals(reader.Value?.ToString(), StringComparison.OrdinalIgnoreCase));
                                    if (want != null)
                                        character.Wants.Add(want);
                                }
                            }
                        }
                        break;
                    case "hailing":
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.WorldWideWrestling.Json.Hailings.json"))
                            using (var hailingsReader = new StreamReader(stream))
                            {
                                var hailings = JsonConvert.DeserializeObject<List<Hailing>>(hailingsReader.ReadToEnd());
                                while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                {
                                    if (reader.TokenType == JsonToken.PropertyName)
                                    {
                                        reader.Read();
                                        var hailingName = reader.Value?.ToString();
                                        character.Hailing = hailings?.FirstOrDefault(h => h.Name.Equals(hailingName, StringComparison.OrdinalIgnoreCase));
                                    }
                                }
                            }
                        }
                        break;
                    case "entrance":
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.WorldWideWrestling.Json.Entrances.json"))
                            using (var entrancesReader = new StreamReader(stream))
                            {
                                var entrances = JsonConvert.DeserializeObject<List<Entrance>>(entrancesReader.ReadToEnd());
                                while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                {
                                    if (reader.TokenType == JsonToken.PropertyName)
                                    {
                                        reader.Read();
                                        var entranceName = reader.Value?.ToString();
                                        character.Entrance = entrances?.FirstOrDefault(e => e.Name.Equals(entranceName, StringComparison.OrdinalIgnoreCase));
                                    }
                                }
                            }
                        }
                        break;
                    case "questions":
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.WorldWideWrestling.Json.Questions.json"))
                            using (var questionsReader = new StreamReader(stream))
                            {
                                var questions = JsonConvert.DeserializeObject<List<Question>>(questionsReader.ReadToEnd());
                                character.Questions = [];
                                while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                {
                                    if (reader.TokenType == JsonToken.PropertyName)
                                    {
                                        reader.Read(); // move to gimmick's question object
                                        if (reader.TokenType == JsonToken.StartObject)
                                        {
                                            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                            {
                                                if (reader.TokenType == JsonToken.PropertyName)
                                                {
                                                    var questionName = reader.Value?.ToString();
                                                    reader.Read();
                                                    var answer = reader.Value?.ToString();
                                                    var question = questions?.FirstOrDefault(q => q.Name.Equals(questionName, StringComparison.OrdinalIgnoreCase));
                                                    if (question != null)
                                                    {
                                                        question.Description = answer ?? "";
                                                        character.Questions.Add(question);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "moves":
                        if (reader.TokenType == JsonToken.StartArray)
                        {
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.WorldWideWrestling.Json.Moves.json"))
                            using (var movesReader = new StreamReader(stream))
                            {
                                var moves = JsonConvert.DeserializeObject<List<Move>>(movesReader.ReadToEnd());
                                character.Moves = new List<Move>();

                                while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                {
                                    if (reader.TokenType != JsonToken.StartObject)
                                        continue;

                                    string moveName = null;
                                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                    {
                                        if (reader.TokenType == JsonToken.PropertyName && reader.Value?.ToString() == "value")
                                        {
                                            reader.Read();
                                            moveName = reader.Value?.ToString();
                                        }
                                    }

                                    if (moveName != null)
                                    {
                                        var move = moves?.FirstOrDefault(m => m.Name.Equals(moveName, StringComparison.OrdinalIgnoreCase));
                                        if (move != null)
                                            character.Moves.Add(move);
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        reader.Skip();
                        break;
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
