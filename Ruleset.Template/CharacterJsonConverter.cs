using Newtonsoft.Json;
using System.Globalization;

namespace Template
{
    internal class CharacterJsonConverter : JsonConverter<Character>
    {
        TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

        public override Character ReadJson(JsonReader reader, Type typeToConvert, Character? existing, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, Character? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
