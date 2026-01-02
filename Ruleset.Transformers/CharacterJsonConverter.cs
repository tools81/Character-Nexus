using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transformers
{
    internal class CharacterJsonConverter : JsonConverter<Character>
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
