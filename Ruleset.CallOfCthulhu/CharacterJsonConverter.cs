using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Utility;

namespace CallOfCthulhu
{
    public class CharacterJsonConverter : JsonConverter<Character>
    {
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

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = reader.Value?.ToString() ?? string.Empty;
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
                        case "pronoun":
                            character.Pronoun = reader.Value?.ToString() ?? "";
                            break;
                        case "birthplace":
                            character.Birthplace = reader.Value?.ToString() ?? "";
                            break;
                        case "residence":
                            character.Residence = reader.Value?.ToString() ?? "";
                            break;
                        case "era":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.CallOfCthulhu.Json.Eras.json"))
                            {
                                if (stream != null)
                                {
                                    using (var sr = new StreamReader(stream))
                                    {
                                        var eras = JsonConvert.DeserializeObject<List<Era>>(sr.ReadToEnd());
                                        character.Era = eras?.FirstOrDefault(e => e.Name.Equals(reader.Value?.ToString(), StringComparison.OrdinalIgnoreCase));
                                    }
                                }
                            }
                            break;
                        case "age":
                        case "characteristics":
                            if (reader.TokenType == JsonToken.StartObject)
                                reader.Skip();
                            break;
                        case "damageBonus":
                            character.DamageBonus = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;
                            break;
                        case "build":
                            character.Build = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;
                            break;
                        case "hitPoints":
                            character.HitPoints = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;
                            break;
                        case "movement":
                            character.Movement = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;
                            break;
                        case "sanityPoints":
                            character.SanityPoints = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;
                            break;
                        case "magicPoints":
                            character.MagicPoints = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;
                            break;
                        case "description":
                            character.Description = reader.Value?.ToString() ?? "";
                            break;
                        case "ideology":
                            character.Ideology = reader.Value?.ToString() ?? "";
                            break;
                        case "significantPeople":
                            character.SignificantPeople = reader.Value?.ToString() ?? "";
                            break;
                        case "meaningfulLocations":
                            character.MeaningfulLocations = reader.Value?.ToString() ?? "";
                            break;
                        case "treasuredPosessions":
                            character.TreasuredPosessions = reader.Value?.ToString() ?? "";
                            break;
                        case "traits":
                            character.Traits = reader.Value?.ToString() ?? "";
                            break;
                        case "scars":
                            character.Scars = reader.Value?.ToString() ?? "";
                            break;
                        case "encounters":
                            character.Encounters = reader.Value?.ToString() ?? "";
                            break;
                        case "story":
                            character.Story = reader.Value?.ToString() ?? "";
                            break;
                        case "skills":
                            if (reader.TokenType == JsonToken.StartObject)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.CallOfCthulhu.Json.Skills.json"))
                                {
                                    if (stream != null)
                                    {
                                        using (var sr = new StreamReader(stream))
                                        {
                                            var allSkills = JsonConvert.DeserializeObject<List<Skill>>(sr.ReadToEnd());
                                            character.Skills = new List<Skill>();

                                            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                            {
                                                if (reader.TokenType == JsonToken.PropertyName)
                                                {
                                                    var skillName = reader.Value?.ToString();
                                                    reader.Read();

                                                    if (reader.TokenType == JsonToken.StartObject)
                                                    {
                                                        reader.Skip();
                                                        continue;
                                                    }

                                                    var value = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;
                                                    var skill = allSkills?.FirstOrDefault(s => s.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase));
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
                            }
                            break;
                        case "creditRating":
                            character.CreditRating = reader.Value?.ToString() ?? "";
                            break;
                        case "spendingLevel":
                            character.SpendingLevel = reader.Value?.ToString() ?? "";
                            break;
                        case "cash":
                            character.Cash = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;
                            break;
                        case "assets":
                            character.Assets = reader.Value?.ToString() ?? "";
                            break;
                        case "notes":
                            character.Notes = reader.Value?.ToString() ?? "";
                            break;
                        case "phobias":
                            if (reader.TokenType == JsonToken.StartArray)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.CallOfCthulhu.Json.Phobias.json"))
                                {
                                    if (stream != null)
                                    {
                                        using (var sr = new StreamReader(stream))
                                        {
                                            var allPhobias = JsonConvert.DeserializeObject<List<Phobia>>(sr.ReadToEnd());
                                            character.Phobias = new List<Phobia>();

                                            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                            {
                                                var phobiaName = reader.Value?.ToString();
                                                var phobia = allPhobias?.FirstOrDefault(p => p.Name.Equals(phobiaName, StringComparison.OrdinalIgnoreCase));
                                                if (phobia != null) character.Phobias.Add(phobia);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "manias":
                            if (reader.TokenType == JsonToken.StartArray)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.CallOfCthulhu.Json.Manias.json"))
                                {
                                    if (stream != null)
                                    {
                                        using (var sr = new StreamReader(stream))
                                        {
                                            var allManias = JsonConvert.DeserializeObject<List<Mania>>(sr.ReadToEnd());
                                            character.Manias = new List<Mania>();

                                            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                            {
                                                var maniaName = reader.Value?.ToString();
                                                var mania = allManias?.FirstOrDefault(m => m.Name.Equals(maniaName, StringComparison.OrdinalIgnoreCase));
                                                if (mania != null) character.Manias.Add(mania);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "spells":
                            if (reader.TokenType == JsonToken.StartArray)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.CallOfCthulhu.Json.Spells.json"))
                                {
                                    if (stream != null)
                                    {
                                        using (var sr = new StreamReader(stream))
                                        {
                                            var allSpells = JsonConvert.DeserializeObject<List<Spell>>(sr.ReadToEnd());
                                            character.Spells = new List<Spell>();

                                            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                            {
                                                var spellName = reader.Value?.ToString();
                                                var spell = allSpells?.FirstOrDefault(s => s.Name.Equals(spellName, StringComparison.OrdinalIgnoreCase));
                                                if (spell != null) character.Spells.Add(spell);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "weapons":
                            if (reader.TokenType == JsonToken.StartArray)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.CallOfCthulhu.Json.Weapons.json"))
                                {
                                    if (stream != null)
                                    {
                                        using (var sr = new StreamReader(stream))
                                        {
                                            var allWeapons = JsonConvert.DeserializeObject<List<Weapon>>(sr.ReadToEnd());
                                            character.Weapons = new List<Weapon>();

                                            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                            {
                                                var weaponName = reader.Value?.ToString();
                                                var weapon = allWeapons?.FirstOrDefault(w => w.Name.Equals(weaponName, StringComparison.OrdinalIgnoreCase));
                                                if (weapon != null) character.Weapons.Add(weapon);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "equipments":
                            if (reader.TokenType == JsonToken.StartArray)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.CallOfCthulhu.Json.Equipments.json"))
                                {
                                    if (stream != null)
                                    {
                                        using (var sr = new StreamReader(stream))
                                        {
                                            var allEquipments = JsonConvert.DeserializeObject<List<Equipment>>(sr.ReadToEnd());
                                            character.Equipments = new List<Equipment>();

                                            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                            {
                                                var equipName = reader.Value?.ToString();
                                                var equipment = allEquipments?.FirstOrDefault(e => e.Name.Equals(equipName, StringComparison.OrdinalIgnoreCase));
                                                if (equipment != null) character.Equipments.Add(equipment);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "choice":
                            if (reader.TokenType == JsonToken.StartObject)
                                reader.Skip();
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