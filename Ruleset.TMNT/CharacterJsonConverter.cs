using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Utility;

namespace TMNT
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
                        case "origin":
                            character.Origin = reader.Value?.ToString() ?? "";
                            break;
                        case "disposition":
                            character.Disposition = reader.Value?.ToString() ?? "";
                            break;
                        case "gender":
                            character.Gender = reader.Value?.ToString() ?? "";
                            break;
                        case "age":
                            character.Age = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;
                            break;
                        case "weight":
                            character.Weight = reader.Value?.ToString() ?? "";
                            break;
                        case "height":
                            character.Height = reader.Value?.ToString() ?? "";
                            break;
                        case "size":
                            character.Size = reader.Value?.ToString() ?? "";
                            break;
                        case "level":
                            character.Level = int.TryParse(reader.Value?.ToString(), out n) ? n : 1;
                            break;
                        case "xp":
                            character.XP = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;
                            break;
                        case "hitPoints":
                            character.HitPoints = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;
                            break;
                        case "sdc":
                            character.SDC = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;
                            break;
                        case "notes":
                            character.Notes = reader.Value?.ToString() ?? "";
                            break;
                        case "animal":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Animals.json"))
                            {
                                if (stream != null)
                                {
                                    using (var sr = new StreamReader(stream))
                                    {
                                        var animals = JsonConvert.DeserializeObject<List<Animal>>(sr.ReadToEnd());
                                        character.Animal = animals?.FirstOrDefault(a => a.Name.Equals(reader.Value?.ToString(), StringComparison.OrdinalIgnoreCase))!;
                                    }
                                }
                            }
                            break;
                        case "alignment":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Alignments.json"))
                            {
                                if (stream != null)
                                {
                                    using (var sr = new StreamReader(stream))
                                    {
                                        var alignments = JsonConvert.DeserializeObject<List<Alignment>>(sr.ReadToEnd());
                                        character.Alignment = alignments?.FirstOrDefault(a => a.Name.Equals(reader.Value?.ToString(), StringComparison.OrdinalIgnoreCase))!;
                                    }
                                }
                            }
                            break;
                        case "education":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Educations.json"))
                            {
                                if (stream != null)
                                {
                                    using (var sr = new StreamReader(stream))
                                    {
                                        var educations = JsonConvert.DeserializeObject<List<Education>>(sr.ReadToEnd());
                                        character.Education = educations?.FirstOrDefault(e => e.Name.Equals(reader.Value?.ToString(), StringComparison.OrdinalIgnoreCase))!;
                                    }
                                }
                            }
                            break;
                        case "mutation":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Mutations.json"))
                            {
                                if (stream != null)
                                {
                                    using (var sr = new StreamReader(stream))
                                    {
                                        var mutations = JsonConvert.DeserializeObject<List<Mutation>>(sr.ReadToEnd());
                                        character.Mutation = mutations?.FirstOrDefault(m => m.Name.Equals(reader.Value?.ToString(), StringComparison.OrdinalIgnoreCase))!;
                                    }
                                }
                            }
                            break;
                        case "organization":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Organizations.json"))
                            {
                                if (stream != null)
                                {
                                    using (var sr = new StreamReader(stream))
                                    {
                                        var organizations = JsonConvert.DeserializeObject<List<Organization>>(sr.ReadToEnd());
                                        character.Organization = organizations?.FirstOrDefault(o => o.Name.Equals(reader.Value?.ToString(), StringComparison.OrdinalIgnoreCase))!;
                                    }
                                }
                            }
                            break;
                        case "attributes":
                            if (reader.TokenType == JsonToken.StartObject)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Attributes.json"))
                                {
                                    if (stream != null)
                                    {
                                        using (var sr = new StreamReader(stream))
                                        {
                                            var allAttributes = JsonConvert.DeserializeObject<List<Attribute>>(sr.ReadToEnd());
                                            character.Attributes = new List<Attribute>();

                                            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                            {
                                                if (reader.TokenType == JsonToken.PropertyName)
                                                {
                                                    var attrName = reader.Value?.ToString();
                                                    reader.Read();
                                                    var value = int.TryParse(reader.Value?.ToString(), out n) ? n : 0;

                                                    var attr = allAttributes?.FirstOrDefault(a => a.Name.Equals(attrName, StringComparison.OrdinalIgnoreCase));
                                                    if (attr != null)
                                                    {
                                                        attr.Value = value;
                                                        character.Attributes.Add(attr);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "biped":
                            if (reader.TokenType == JsonToken.StartObject)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Bipeds.json"))
                                {
                                    if (stream != null)
                                    {
                                        using (var sr = new StreamReader(stream))
                                        {
                                            var bipeds = JsonConvert.DeserializeObject<List<Biped>>(sr.ReadToEnd());

                                            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                            {
                                                if (reader.TokenType == JsonToken.PropertyName)
                                                {
                                                    var animalName = reader.Value?.ToString();
                                                    reader.Read();
                                                    var bipedName = reader.Value?.ToString();
                                                    var biped = bipeds?.FirstOrDefault(b =>
                                                        b.Animal.Equals(animalName, StringComparison.OrdinalIgnoreCase) &&
                                                        b.Name.Equals(bipedName, StringComparison.OrdinalIgnoreCase));
                                                    if (biped != null) character.Biped = biped;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "hand":
                            if (reader.TokenType == JsonToken.StartObject)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Hands.json"))
                                {
                                    if (stream != null)
                                    {
                                        using (var sr = new StreamReader(stream))
                                        {
                                            var hands = JsonConvert.DeserializeObject<List<Hand>>(sr.ReadToEnd());

                                            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                            {
                                                if (reader.TokenType == JsonToken.PropertyName)
                                                {
                                                    var animalName = reader.Value?.ToString();
                                                    reader.Read();
                                                    var handName = reader.Value?.ToString();
                                                    var hand = hands?.FirstOrDefault(h =>
                                                        h.Animal.Equals(animalName, StringComparison.OrdinalIgnoreCase) &&
                                                        h.Name.Equals(handName, StringComparison.OrdinalIgnoreCase));
                                                    if (hand != null) character.Hand = hand;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "look":
                            if (reader.TokenType == JsonToken.StartObject)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Looks.json"))
                                {
                                    if (stream != null)
                                    {
                                        using (var sr = new StreamReader(stream))
                                        {
                                            var looks = JsonConvert.DeserializeObject<List<Look>>(sr.ReadToEnd());

                                            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                            {
                                                if (reader.TokenType == JsonToken.PropertyName)
                                                {
                                                    var animalName = reader.Value?.ToString();
                                                    reader.Read();
                                                    var lookName = reader.Value?.ToString();
                                                    var look = looks?.FirstOrDefault(l =>
                                                        l.Animal.Equals(animalName, StringComparison.OrdinalIgnoreCase) &&
                                                        l.Name.Equals(lookName, StringComparison.OrdinalIgnoreCase));
                                                    if (look != null) character.Look = look;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "naturalWeapon":
                            if (reader.TokenType == JsonToken.StartObject)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.NaturalWeapons.json"))
                                {
                                    if (stream != null)
                                    {
                                        using (var sr = new StreamReader(stream))
                                        {
                                            var naturalWeapons = JsonConvert.DeserializeObject<List<NaturalWeapon>>(sr.ReadToEnd());

                                            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                            {
                                                if (reader.TokenType == JsonToken.PropertyName)
                                                {
                                                    var animalName = reader.Value?.ToString();
                                                    reader.Read();
                                                    var weaponName = reader.Value?.ToString();
                                                    var naturalWeapon = naturalWeapons?.FirstOrDefault(nw =>
                                                        nw.Animal.Equals(animalName, StringComparison.OrdinalIgnoreCase) &&
                                                        nw.Name.Equals(weaponName, StringComparison.OrdinalIgnoreCase));
                                                    if (naturalWeapon != null) character.NaturalWeapon = naturalWeapon;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "speech":
                            if (reader.TokenType == JsonToken.StartObject)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Speechs.json"))
                                {
                                    if (stream != null)
                                    {
                                        using (var sr = new StreamReader(stream))
                                        {
                                            var speeches = JsonConvert.DeserializeObject<List<Speech>>(sr.ReadToEnd());

                                            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                            {
                                                if (reader.TokenType == JsonToken.PropertyName)
                                                {
                                                    var animalName = reader.Value?.ToString();
                                                    reader.Read();
                                                    var speechName = reader.Value?.ToString();
                                                    var speech = speeches?.FirstOrDefault(s =>
                                                        s.Animal.Equals(animalName, StringComparison.OrdinalIgnoreCase) &&
                                                        s.Name.Equals(speechName, StringComparison.OrdinalIgnoreCase));
                                                    if (speech != null) character.Speech = speech;
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
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Skills.json"))
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
                        case "vehicle":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Vehicles.json"))
                            {
                                if (stream != null)
                                {
                                    using (var sr = new StreamReader(stream))
                                    {
                                        var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(sr.ReadToEnd());
                                        character.Vehicle = vehicles?.FirstOrDefault(v => v.Name.Equals(reader.Value?.ToString(), StringComparison.OrdinalIgnoreCase))!;
                                    }
                                }
                            }
                            break;
                        case "armor":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Armors.json"))
                            {
                                if (stream != null)
                                {
                                    using (var sr = new StreamReader(stream))
                                    {
                                        var armors = JsonConvert.DeserializeObject<List<Armor>>(sr.ReadToEnd());
                                        character.Armor = armors?.FirstOrDefault(a => a.Name.Equals(reader.Value?.ToString(), StringComparison.OrdinalIgnoreCase))!;
                                    }
                                }
                            }
                            break;
                        case "psionics":
                            if (reader.TokenType == JsonToken.StartArray)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Psionics.json"))
                                {
                                    if (stream != null)
                                    {
                                        using (var sr = new StreamReader(stream))
                                        {
                                            var allPsionics = JsonConvert.DeserializeObject<List<Psionic>>(sr.ReadToEnd());
                                            character.Psionics = new List<Psionic>();

                                            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                            {
                                                var psiName = reader.Value?.ToString();
                                                var psionic = allPsionics?.FirstOrDefault(p => p.Name.Equals(psiName, StringComparison.OrdinalIgnoreCase));
                                                if (psionic != null) character.Psionics.Add(psionic);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "weapons":
                            if (reader.TokenType == JsonToken.StartArray)
                            {
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Weapons.json"))
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
                                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.TMNT.Json.Equipment.json"))
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
