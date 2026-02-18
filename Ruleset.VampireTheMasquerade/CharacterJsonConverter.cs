using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Utility;

namespace VampireTheMasquerade
{
    public class CharacterJsonConverter : JsonConverter<Character>
    {
        public override Character ReadJson(JsonReader reader, Type objectType, Character? existing, bool hasExistingValue, JsonSerializer serializer)
        {
            var character = new Character() { Name = "" };
            var n = 0;
            var dateTime = DateTime.MinValue;

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
                        case "concept":
                            character.Concept = reader.Value.ToString();
                            break;
                        case "age":
                            character.Age = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "dateOfBirth":
                            character.DateOfBirth = DateTime.TryParse(reader.Value.ToString(), out dateTime) ? dateTime : DateTime.MinValue;
                            break;
                        case "dateOfDeath":
                            character.DateOfDeath = DateTime.TryParse(reader.Value.ToString(), out dateTime) ? dateTime : DateTime.MinValue;
                            break;
                        case "distinguishingFeatures":
                            character.DistinguishingFeatures = reader.Value.ToString();
                            break;
                        case "appearance":
                            character.Appearance = reader.Value.ToString();
                            break;
                        case "history":
                            character.History = reader.Value.ToString();
                            break;
                        case "chronicle":
                            character.Chronicle = reader.Value.ToString();
                            break;
                        case "ambition":
                            character.Ambition = reader.Value.ToString();
                            break;
                        case "sire":
                            character.Sire = reader.Value.ToString();
                            break;
                        case "desire":
                            character.Desire = reader.Value.ToString();
                            break;
                        case "health":
                            character.Health = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "willpower":
                            character.Willpower = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "humanity":
                            character.Humanity = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "hunger":
                            character.Hunger = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "bloodPotency":
                            character.BloodPotency = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "totalExperience":
                            character.TotalExperience = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "spentExperience":
                            character.SpentExperience = int.TryParse(reader.Value.ToString(), out n) ? n : 0;
                            break;
                        case "attributes":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for attributes");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.VampireTheMasquerade.Json.Attributes.json"))
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
                        case "clan":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.VampireTheMasquerade.Json.Clans.json"))
                            {
                                using (var clansReader = new StreamReader(stream))
                                {
                                    var jsonContent = clansReader.ReadToEnd();
                                    var clans = JsonTo.List<Clan>(jsonContent);

                                    var clan = clans.Find(o => o.Name.ToLower() == reader.Value.ToString().ToLower());

                                    //Read the end object
                                    reader.Read();

                                    character.Clan = clan;
                                }
                            }
                            break;
                        case "predator":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.VampireTheMasquerade.Json.Predators.json"))
                            {
                                using (var predatorsReader = new StreamReader(stream))
                                {
                                    var jsonContent = predatorsReader.ReadToEnd();
                                    var predators = JsonTo.List<Predator>(jsonContent);

                                    var predator = predators.Find(o => o.Name.ToLower() == reader.Value.ToString().ToLower());

                                    //Read the end object
                                    reader.Read();

                                    character.Predator = predator;
                                }
                            }
                            break;
                        case "generation":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.VampireTheMasquerade.Json.Generations.json"))
                            {
                                using (var generationsReader = new StreamReader(stream))
                                {
                                    var jsonContent = generationsReader.ReadToEnd();
                                    var generations = JsonTo.List<Generation>(jsonContent);

                                    var generation = generations.Find(o => o.Name.ToLower() == reader.Value.ToString().ToLower());

                                    //Read the end object
                                    reader.Read();

                                    character.Generation = generation;
                                }
                            }
                            break;
                        case "coterie":
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.VampireTheMasquerade.Json.Coteries.json"))
                            {
                                using (var coteriesReader = new StreamReader(stream))
                                {
                                    var jsonContent = coteriesReader.ReadToEnd();
                                    var coteries = JsonTo.List<Coterie>(jsonContent);

                                    var coterie = coteries.Find(o => o.Name.ToLower() == reader.Value.ToString().ToLower());

                                    //Read the end object
                                    reader.Read();

                                    character.Coterie = coterie;
                                }
                            }
                            break;
                        case "disciplines":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for disciplines");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.VampireTheMasquerade.Json.Disciplines.json"))
                            {
                                using (var disciplinesReader = new StreamReader(stream))
                                {
                                    var jsonContent = disciplinesReader.ReadToEnd();
                                    var disciplines = JsonTo.List<Discipline>(jsonContent);

                                    character.Disciplines = new List<Discipline>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                    {
                                        var propName = reader.Value.ToString();
                                        reader.Read();
                                        var value = int.Parse(reader.Value.ToString());

                                        var found = disciplines.Find(p => p.Name.ToLower() == propName.ToLower());
                                        found.Value = value;
                                        character.Disciplines.Add(found);
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
                        case "skills":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for skills");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.VampireTheMasquerade.Json.Skills.json"))
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
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for specialities");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.VampireTheMasquerade.Json.Specialties.json"))
                            {
                                using (var specialtiesReader = new StreamReader(stream))
                                {
                                    var jsonContent = specialtiesReader.ReadToEnd();
                                    var specialties = JsonTo.List<Specialty>(jsonContent);

                                    character.Specialties = new List<Specialty>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                    {
                                        var propName = reader.Value.ToString();
                                        reader.Read();

                                        var found = specialties.Find(p => p.Name.ToLower() == propName.ToLower());
                                        character.Specialties.Add(found);
                                    }
                                }
                            }
                            break;
                        case "rituals":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for rituals");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Marvel.Json.Rituals.json"))
                            {
                                using (var ritualsReader = new StreamReader(stream))
                                {
                                    var jsonContent = ritualsReader.ReadToEnd();
                                    var rituals = JsonTo.List<Ritual>(jsonContent);

                                    character.Rituals = new List<Ritual>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        var found = rituals.Find(w => w.Name.ToLower() == (reader.Value.ToString()).ToLower());

                                        reader.Read();

                                        character.Rituals.Add(found);
                                    }
                                }
                            }
                            break;
                        case "backgrounds":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for backgrounds");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.VampireTheMasquerade.Json.Backgrounds.json"))
                            {
                                using (var backgroundsReader = new StreamReader(stream))
                                {
                                    var jsonContent = backgroundsReader.ReadToEnd();
                                    var backgrounds = JsonTo.List<Background>(jsonContent);

                                    character.Backgrounds = new List<Background>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                    {
                                        var propName = reader.Value.ToString();
                                        reader.Read();
                                        var value = int.Parse(reader.Value.ToString());

                                        var found = backgrounds.Find(p => p.Name.ToLower() == propName.ToLower());
                                        found.Value = value;
                                        character.Backgrounds.Add(found);
                                    }
                                }
                            }
                            break;
                        case "merits":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for merits");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.VampireTheMasquerade.Json.Merits.json"))
                            {
                                using (var meritsReader = new StreamReader(stream))
                                {
                                    var jsonContent = meritsReader.ReadToEnd();
                                    var merits = JsonTo.List<Merit>(jsonContent);

                                    character.Merits = new List<Merit>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                    {
                                        var propName = reader.Value.ToString();
                                        reader.Read();
                                        var value = int.Parse(reader.Value.ToString());

                                        var found = merits.Find(p => p.Name.ToLower() == propName.ToLower());
                                        found.Value = value;
                                        character.Merits.Add(found);
                                    }
                                }
                            }
                            break;
                        case "flaws":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for flaws");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.VampireTheMasquerade.Json.Flaws.json"))
                            {
                                using (var flawsReader = new StreamReader(stream))
                                {
                                    var jsonContent = flawsReader.ReadToEnd();
                                    var flaws = JsonTo.List<Flaw>(jsonContent);

                                    character.Flaws = new List<Flaw>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                    {
                                        var propName = reader.Value.ToString();
                                        reader.Read();
                                        var value = int.Parse(reader.Value.ToString());

                                        var found = flaws.Find(p => p.Name.ToLower() == propName.ToLower());
                                        found.Value = value;
                                        character.Flaws.Add(found);
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
                                        var found = weapons.Find(w => w.Name.ToLower() == (reader.Value.ToString()).ToLower());

                                        reader.Read();

                                        character.Weapons.Add(found);
                                    }
                                }
                            }
                            break;
                        case "armors":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for armors");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Marvel.Json.Armors.json"))
                            {
                                using (var armorsReader = new StreamReader(stream))
                                {
                                    var jsonContent = armorsReader.ReadToEnd();
                                    var armors = JsonTo.List<Armor>(jsonContent);

                                    character.Armors = new List<Armor>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        var found = armors.Find(w => w.Name.ToLower() == (reader.Value.ToString()).ToLower());

                                        reader.Read();

                                        character.Armors.Add(found);
                                    }
                                }
                            }
                            break;
                        case "gears":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for gears");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.Marvel.Json.Gears.json"))
                            {
                                using (var gearsReader = new StreamReader(stream))
                                {
                                    var jsonContent = gearsReader.ReadToEnd();
                                    var gears = JsonTo.List<Gear>(jsonContent);

                                    character.Gears = new List<Gear>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        var found = gears.Find(w => w.Name.ToLower() == (reader.Value.ToString()).ToLower());

                                        reader.Read();

                                        character.Gears.Add(found);
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

            writer.WritePropertyName("notes");
            writer.WriteValue(character.Notes);

            writer.WritePropertyName("concept");
            writer.WriteValue(character.Concept);

            writer.WritePropertyName("age");
            writer.WriteValue(character.Age);

            writer.WritePropertyName("dateOfBirth");
            writer.WriteValue(character.DateOfBirth);

            writer.WritePropertyName("dateOfDeath");
            writer.WriteValue(character.DateOfDeath);

            writer.WritePropertyName("appearance");
            writer.WriteValue(character.Appearance);

            writer.WritePropertyName("distinguishingFeatures");
            writer.WriteValue(character.DistinguishingFeatures);

            writer.WritePropertyName("history");
            writer.WriteValue(character.History);

            writer.WritePropertyName("predator");
            writer.WriteStartObject();
            writer.WriteValue(character.Predator.Name);
            writer.WriteEndObject();

            writer.WritePropertyName("chronicle");
            writer.WriteValue(character.Chronicle);

            writer.WritePropertyName("ambition");
            writer.WriteValue(character.Ambition);

            writer.WritePropertyName("clan");
            writer.WriteStartObject();
            writer.WriteValue(character.Clan.Name);
            writer.WriteEndObject();

            writer.WritePropertyName("coterie");
            writer.WriteStartObject();
            writer.WriteValue(character.Coterie.Name);
            writer.WriteEndObject();

            writer.WritePropertyName("sire");
            writer.WriteValue(character.Sire);

            writer.WritePropertyName("desire");
            writer.WriteValue(character.Desire);

            writer.WritePropertyName("generation");
            writer.WriteStartObject();
            writer.WriteValue(character.Generation.Name);
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

            writer.WritePropertyName("health");
            writer.WriteValue(character.Health);

            writer.WritePropertyName("willpower");
            writer.WriteValue(character.Willpower);

            writer.WritePropertyName("hunger");
            writer.WriteValue(character.Hunger);

            writer.WritePropertyName("humanity");
            writer.WriteValue(character.Humanity);

            writer.WritePropertyName("chronicleTenets");
            writer.WriteValue(character.ChronicleTenets);

            writer.WritePropertyName("touchstonesConvictions");
            writer.WriteValue(character.TouchstonesConvictions);

            writer.WritePropertyName("bloodPotency");
            writer.WriteValue(character.BloodPotency);

            writer.WritePropertyName("totalExperience");
            writer.WriteValue(character.TotalExperience);

            writer.WritePropertyName("spentExperience");
            writer.WriteValue(character.SpentExperience);

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

            writer.WritePropertyName("disciplines");
            writer.WriteStartArray();
            foreach (var discipline in character.Disciplines)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(discipline.Name);
                writer.WriteValue(discipline.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("powers");
            writer.WriteStartArray();
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.VampireTheMasquerade.Json.Powers.json"))
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

            writer.WritePropertyName("rituals");
            writer.WriteStartArray();
            foreach (var ritual in character.Rituals)
            {
                writer.WriteStartObject();
                writer.WriteValue(ritual.Name);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("advantages");
            writer.WriteStartArray();
            foreach (var advantage in character.Advantages)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(advantage.Name);
                writer.WriteValue(advantage.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("merits");
            writer.WriteStartArray();
            foreach (var merit in character.Merits)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(merit.Name);
                writer.WriteValue(merit.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("flaws");
            writer.WriteStartArray();
            foreach (var flaw in character.Flaws)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(flaw.Name);
                writer.WriteValue(flaw.Value);
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

            writer.WritePropertyName("armors");
            writer.WriteStartArray();
            foreach (var armor in character.Armors)
            {
                writer.WriteStartObject();
                writer.WriteValue(armor.Name);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

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
        }
    }
}
