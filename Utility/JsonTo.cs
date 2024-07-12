using Newtonsoft.Json;
using System.Collections.Generic;

namespace Utility
{
    public static class JsonTo
    {
        public static Dictionary<string, T> Dictionary<T>(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, T>>(json);
        }

        public static List<T> List<T>(string json)
        {
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        public static IEnumerable<T> IEnumerable<T>(string json)
        {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
        }
    }
}
