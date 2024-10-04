using System.Reflection;
using Newtonsoft.Json;
using Utility;
using System.Globalization;
using System;

namespace VampireTheMasquerade
{
    public class CharacterJsonConverter : JsonConverter<Character>
    {
        public override Character ReadJson(JsonReader reader, Type objectType, Character existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, Character value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
