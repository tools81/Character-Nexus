using System.Reflection;
using Newtonsoft.Json;
using Utility;
using System.Globalization;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace EverydayHeroes
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

                if (reader.TokenType != JsonToken.PropertyName) 
                    continue;

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
                    case "level":
                        character.Level = Convert.ToInt32(reader.Value);
                        break;
                    case "wealthLevel":
                        //character.WealthLevel = Convert.ToInt32(reader.Value);
                        break;
                    case "languages":
                        character.Languages = reader.Value.ToString();
                        break;
                    case "motivations":
                        character.Motivations = reader.Value.ToString();
                        break;
                    case "attachments":
                        character.Attachments = reader.Value.ToString();
                        break;
                    case "beliefs":
                        character.Beliefs = reader.Value.ToString();
                        break;
                    case "ancestry":
                        character.Ancestry = reader.Value.ToString();
                        break;
                    case "quirks":
                        character.Quirks = reader.Value.ToString(); 
                        break;
                    case "virtues":
                        character.Virtues = reader.Value.ToString();
                        break;
                    case "flaws":
                        character.Flaws = reader.Value.ToString();
                        break;
                    case "role":
                        character.Role = reader.Value.ToString();
                        break;
                    case "weight":
                        character.Weight = reader.Value.ToString();
                        break;
                    case "height":
                        character.Height = reader.Value.ToString();
                        break;
                    case "hair":
                        character.Hair = reader.Value.ToString();
                        break;
                    case "skin":
                        character.Skin = reader.Value.ToString();
                        break;
                    case "eyes":
                        character.Eyes = reader.Value.ToString();
                        break;
                    case "age":
                        character.Age = reader.Value.ToString();
                        break;
                    case "maritalStatus":
                        character.MaritalStatus = reader.Value.ToString();
                        break;
                    case "pronouns":
                        character.Pronouns = reader.Value.ToString();
                        break;
                    case "biography":
                        character.Biography = reader.Value.ToString();
                        break;
                    case "notes":
                        character.Notes = reader.Value.ToString();
                        break;
                    case "savingThrowProficiency":
                        if (reader.TokenType != JsonToken.StartArray)
                        {
                            throw new JsonException("Expected StartArray token for saving throw proficiencies");
                        }

                        character.SavingThrowProficiency = new List<string>();

                        while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                        {
                            if (reader.TokenType != JsonToken.String)
                                continue;

                            character.SavingThrowProficiency.Add(reader.Value.ToString());
                        }
                        break;
                    case "equipmentProficiency":
                        if (reader.TokenType != JsonToken.StartArray)
                        {
                            throw new JsonException("Expected StartArray token for equipment proficiencies");
                        }

                        character.EquipmentProficiency = new List<string>();

                        while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                        {
                            if (reader.TokenType != JsonToken.String)
                                continue;

                            character.EquipmentProficiency.Add(reader.Value.ToString());
                        }
                        break;
                    case "archetype":
                        if (reader.TokenType != JsonToken.String)
                        {
                            throw new JsonException("Expected String token for archetype");
                        }

                        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Archetypes.json"))
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
                    case "class":
                        if (reader.TokenType != JsonToken.StartObject)                        {
                            throw new JsonException("Expected StartObject token for class");
                        }

                        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Classes.json"))
                        {
                            using (var classesReader = new StreamReader(stream))
                            {
                                var jsonContent = classesReader.ReadToEnd();
                                var classes = JsonTo.List<Class>(jsonContent);

                                while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                {
                                    var propName = reader.Value.ToString();
                                    reader.Read();
                                    var value = reader.Value?.ToString();

                                    if (value != null)
                                    {
                                        var found = classes.Find(p => p.Name.ToLower() == value.ToLower() && p.Archetype.ToLower() == propName.ToLower());
                                        character.Class = found;
                                    }          
                                }
                            }
                        }
                        break;
                    case "background":
                        if (reader.TokenType != JsonToken.String)
                        {
                            throw new JsonException("Expected String token for background");
                        }

                        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Backgrounds.json"))
                        {
                            using (var backgroundsReader = new StreamReader(stream))
                            {
                                var jsonContent = backgroundsReader.ReadToEnd();
                                var backgrounds = JsonTo.List<Background>(jsonContent);

                                var background = backgrounds.Find(o => o.Name.ToLower() == reader.Value.ToString().ToLower());

                                character.Background = background;
                            }
                        }
                        break;
                    case "profession":
                        if (reader.TokenType != JsonToken.String)
                        {
                            throw new JsonException("Expected String token for profession");
                        }

                        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Professions.json"))
                        {
                            using (var professionsReader = new StreamReader(stream))
                            {
                                var jsonContent = professionsReader.ReadToEnd();
                                var professions = JsonTo.List<Profession>(jsonContent);

                                var profession = professions.Find(o => o.Name.ToLower() == reader.Value.ToString().ToLower());

                                character.Profession = profession;
                            }
                        }
                        break;
                    case "attribute":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for attributes");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Attributes.json"))
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
                    case "skills":
                        if (reader.TokenType != JsonToken.StartObject)
                        {
                            throw new JsonException("Expected StartObject token for skills");
                        }

                        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Skills.json"))
                        {
                            using (var skillsReader = new StreamReader(stream))
                            {
                                var jsonContent = skillsReader.ReadToEnd();
                                var skills = JsonTo.List<Skill>(jsonContent);

                                character.Skills = skills;

                                while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                {
                                    if (reader.TokenType != JsonToken.PropertyName)
                                        continue;

                                    string skillTier = reader.Value.ToString();
                                    reader.Read(); // StartObject

                                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                    {
                                        if (reader.TokenType != JsonToken.PropertyName)
                                            continue;

                                        string propName = reader.Value.ToString();
                                        reader.Read();

                                        var skill = skills.Find(p => p.Name.ToLower() == propName.ToLower());

                                        switch (skillTier.ToLower())
                                        {
                                            case "proficient":
                                                character.Skills.FirstOrDefault(s => s.Name.ToLower() == skill.Name.ToLower()).Proficient = Convert.ToBoolean(reader.Value);
                                                break;
                                            case "expertise":
                                                character.Skills.FirstOrDefault(s => s.Name.ToLower() == skill.Name.ToLower()).Expertise = Convert.ToBoolean(reader.Value);
                                                break;
                                        }
                                    }                                    
                                }
                            }
                        }
                        break;
                    case "feat":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for feats");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Feats.json"))
                            {
                                using (var featsReader = new StreamReader(stream))
                                {
                                    var jsonContent = featsReader.ReadToEnd();
                                    var feats = JsonTo.List<Feat>(jsonContent);

                                    character.Feats = new List<Feat>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        if (reader.TokenType != JsonToken.String)
                                            continue;
                                            
                                        var found = feats.Find(f => f.Name.ToLower() == (reader.Value.ToString()).ToLower());
                                        character.Feats.Add(found);
                                    }
                                }
                            }
                        break;
                    case "talent":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for talents");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Talents.json"))
                            {
                                using (var talentsReader = new StreamReader(stream))
                                {
                                    var jsonContent = talentsReader.ReadToEnd();
                                    var talents = JsonTo.List<Talent>(jsonContent);

                                    character.Talents = new List<Talent>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        if (reader.TokenType != JsonToken.String)
                                            continue;
                                            
                                        var found = talents.Find(t => t.Name.ToLower() == reader.Value.ToString().ToLower());
                                        character.Talents.Add(found);
                                    }
                                }
                            }
                        break;
                    case "plan":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for plans");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Plans.json"))
                            {
                                using (var plansReader = new StreamReader(stream))
                                {
                                    var jsonContent = plansReader.ReadToEnd();
                                    var plans = JsonTo.List<Plan>(jsonContent);

                                    character.Plans = new List<Plan>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        if (reader.TokenType != JsonToken.String)
                                            continue;
                                            
                                        var found = plans.Find(p => p.Name.ToLower() == (reader.Value.ToString()).ToLower());
                                        character.Plans.Add(found);
                                    }
                                }
                            }
                        break;
                    case "trick":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for tricks");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Tricks.json"))
                            {
                                using (var tricksReader = new StreamReader(stream))
                                {
                                    var jsonContent = tricksReader.ReadToEnd();
                                    var tricks = JsonTo.List<Trick>(jsonContent);

                                    character.Tricks = new List<Trick>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        if (reader.TokenType != JsonToken.String)
                                            continue;
                                            
                                        var found = tricks.Find(t => t.Name.ToLower() == (reader.Value.ToString()).ToLower());
                                        character.Tricks.Add(found);
                                    }
                                }
                            }
                        break;
                    case "pack":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for packs");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Packs.json"))
                            {
                                using (var packsReader = new StreamReader(stream))
                                {
                                    var jsonContent = packsReader.ReadToEnd();
                                    var packs = JsonTo.List<Pack>(jsonContent);

                                    character.Packs = new List<Pack>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        if (reader.TokenType != JsonToken.String)
                                            continue;
                                            
                                        var found = packs.Find(p => p.Name.ToLower() == (reader.Value.ToString()).ToLower());
                                        character.Packs.Add(found);
                                    }
                                }
                            }
                        break;
                    case "item":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for items");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Items.json"))
                            {
                                using (var itemsReader = new StreamReader(stream))
                                {
                                    var jsonContent = itemsReader.ReadToEnd();
                                    var items = JsonTo.List<Item>(jsonContent);

                                    character.Items = new List<Item>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        if (reader.TokenType != JsonToken.String)
                                            continue;
                                            
                                        var found = items.Find(i => i.Name.ToLower() == (reader.Value.ToString()).ToLower());
                                        character.Items.Add(found);
                                    }
                                }
                            }
                        break;
                    case "weapon":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for weapons");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Weapons.json"))
                            {
                                using (var weaponsReader = new StreamReader(stream))
                                {
                                    var jsonContent = weaponsReader.ReadToEnd();
                                    var weapons = JsonTo.List<Weapon>(jsonContent);

                                    character.Weapons = new List<Weapon>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        if (reader.TokenType != JsonToken.String)
                                            continue;
                                            
                                        var found = weapons.Find(w => w.Name.ToLower() == (reader.Value.ToString()).ToLower());

                                        character.Weapons.Add(found);
                                    }
                                }
                            }
                        break;
                    case "armor":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for armor");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Armors.json"))
                            {
                                using (var armorReader = new StreamReader(stream))
                                {
                                    var jsonContent = armorReader.ReadToEnd();
                                    var armors = JsonTo.List<Armor>(jsonContent);

                                    character.Armors = new List<Armor>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        if (reader.TokenType != JsonToken.String)
                                            continue;
                                            
                                        var found = armors.Find(a => a.Name.ToLower() == (reader.Value.ToString()).ToLower());
                                        character.Armors.Add(found);
                                    }
                                }
                            }
                        break;
                    case "vehicle":
                            if (reader.TokenType != JsonToken.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for vehicles");
                            }

                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Vehicles.json"))
                            {
                                using (var vehiclesReader = new StreamReader(stream))
                                {
                                    var jsonContent = vehiclesReader.ReadToEnd();
                                    var vehicles = JsonTo.List<Vehicle>(jsonContent);

                                    character.Vehicles = new List<Vehicle>();

                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        if (reader.TokenType != JsonToken.String)
                                            continue;
                                            
                                        var found = vehicles.Find(v => v.Name.ToLower() == (reader.Value.ToString()).ToLower());
                                        character.Vehicles.Add(found);
                                    }
                                }
                            }
                        break;
                    case "choice":
                            if (reader.TokenType != JsonToken.StartObject)
                            {
                                throw new JsonException("Expected StartObject token for choices");
                            }

                            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                            {
                                if (reader.TokenType != JsonToken.PropertyName)
                                    continue;

                                string sectionName = (string)reader.Value!;
                                reader.Read(); // StartObject

                                var sectionDict = new Dictionary<string, List<object?>>();
                                ReadSection(reader, sectionDict);

                                character.Choice.Sections[sectionName] = sectionDict;        
                            }
                        break;
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

            writer.WritePropertyName("level");
            writer.WriteValue(character.Level);

            writer.WritePropertyName("languages");
            writer.WriteValue(character.Languages);

            writer.WritePropertyName("motivations");
            writer.WriteValue(character.Motivations);

            writer.WritePropertyName("attachments");
            writer.WriteValue(character.Attachments);

            writer.WritePropertyName("beliefs");
            writer.WriteValue(character.Beliefs);

            writer.WritePropertyName("ancestry");
            writer.WriteValue(character.Ancestry);

            writer.WritePropertyName("quirks");
            writer.WriteValue(character.Quirks);

            writer.WritePropertyName("virtues");
            writer.WriteValue(character.Virtues);

            writer.WritePropertyName("flaws");
            writer.WriteValue(character.Flaws);

            writer.WritePropertyName("role");
            writer.WriteValue(character.Role);

            writer.WritePropertyName("weight");
            writer.WriteValue(character.Weight);

            writer.WritePropertyName("height");
            writer.WriteValue(character.Height);

            writer.WritePropertyName("hair");
            writer.WriteValue(character.Hair);

            writer.WritePropertyName("skin");
            writer.WriteValue(character.Skin);

            writer.WritePropertyName("eyes");
            writer.WriteValue(character.Eyes);

            writer.WritePropertyName("age");
            writer.WriteValue(character.Age);

            writer.WritePropertyName("maritalStatus");
            writer.WriteValue(character.MaritalStatus);

            writer.WritePropertyName("pronouns");
            writer.WriteValue(character.Pronouns);

            writer.WritePropertyName("biography");
            writer.WriteValue(character.Biography);

            writer.WritePropertyName("notes");
            writer.WriteValue(character.Notes);

            writer.WritePropertyName("archetype");
            writer.WriteStartObject();
            writer.WriteValue(character.Archetype.Name);
            writer.WriteEndObject();

            writer.WritePropertyName("class");
            writer.WriteStartObject();
            writer.WriteValue(character.Class.Name);
            writer.WriteEndObject();

            writer.WritePropertyName("background");
            writer.WriteStartObject();
            writer.WriteValue(character.Background.Name);
            writer.WriteEndObject();

            writer.WritePropertyName("profession");
            writer.WriteStartObject();
            writer.WriteValue(character.Profession.Name);
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

            writer.WritePropertyName("feats");
            writer.WriteStartArray();
            foreach (var feat in character.Feats)
            {
                writer.WriteStartObject();
                writer.WriteValue(feat.Name);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("plans");
            writer.WriteStartArray();
            foreach (var plan in character.Plans)
            {
                writer.WriteStartObject();
                writer.WriteValue(plan.Name);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("tricks");
            writer.WriteStartArray();
            foreach (var trick in character.Tricks)
            {
                writer.WriteStartObject();
                writer.WriteValue(trick.Name);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("packs");
            writer.WriteStartArray();
            foreach (var pack in character.Packs)
            {
                writer.WriteStartObject();
                writer.WriteValue(pack.Name);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("items");
            writer.WriteStartArray();
            foreach (var item in character.Items)
            {
                writer.WriteStartObject();
                writer.WriteValue(item.Name);
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
        }

        private static void ReadSection(JsonReader reader,
        Dictionary<string, List<object?>> sectionDict)
        {
            // reader is positioned at StartObject; read until the matching EndObject
            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
            {
                if (reader.TokenType != JsonToken.PropertyName)
                    continue;

                string propertyName = (string)reader.Value!;
                reader.Read();

                if (reader.TokenType == JsonToken.StartArray)
                {
                    // Collect all array elements, preserving nulls (null → default 0 for adjustments)
                    var values = new List<object?>();
                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                    {
                        values.Add(reader.Value);
                    }
                    sectionDict[propertyName] = values;
                }
                else if (reader.TokenType == JsonToken.StartObject)
                {
                    // Flatten nested objects (e.g. skills.proficient.class) into the same dict
                    ReadSection(reader, sectionDict);
                }
            }
        }
    }
}