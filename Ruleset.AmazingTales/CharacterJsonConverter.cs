using System.Reflection;
using Newtonsoft.Json;
using Utility;
using System.Globalization;
using System;

namespace AmazingTales
{
    public class CharacterJsonConverter : JsonConverter<Character>
    {
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
                    string propertyName = (string)reader.Value;
                    reader.Read();

                    switch (propertyName)
                    {
                        case "id":
                            var id = (string)reader.Value;
                            character.Id = id == string.Empty ? Guid.NewGuid() : new Guid(id);
                            break;
                        case "image":
                            character.Image = (string)reader.Value;
                            break;
                        case "name":
                            character.Name = (string)reader.Value;
                            break;
                        case "d12Attribute":
                            character.D12Attribute = (string)reader.Value;
                            break;
                        case "d10Attribute":
                            character.D10Attribute = (string)reader.Value;
                            break;
                        case "d8Attribute":
                            character.D8Attribute = (string)reader.Value;
                            break;
                        case "d6Attribute":
                            character.D6Attribute = (string)reader.Value;
                            break;
                        case "notes":
                            character.Notes = (string)reader.Value;
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

            writer.WritePropertyName("d12Attribute");
            writer.WriteValue(character.D12Attribute);

            writer.WritePropertyName("d10Attribute");
            writer.WriteValue(character.D10Attribute);

            writer.WritePropertyName("d8Attribute");
            writer.WriteValue(character.D8Attribute);

            writer.WritePropertyName("d6Attribute");
            writer.WriteValue(character.D6Attribute);

            writer.WritePropertyName("notes");
            writer.WriteValue(character.Notes);

            writer.WriteEndObject();
            writer.Close();
        }
    }
}
