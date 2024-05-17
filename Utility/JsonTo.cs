using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class JsonTo
    {
        // public static Dictionary<string, IAttribute> ConvertAttribute<IAttribute>(string json)
        // {
        //     //// Deserialize the JSON into a dictionary
        //     //var attributesData = JsonConvert.DeserializeObject<Dictionary<string, IAttribute>>(json);

        //     //// Create a dictionary to hold the parsed data
        //     //var attributesDictionary = new Dictionary<string, IAttribute>();

        //     //foreach (var kvp in attributesData["Attributes"])
        //     //{
        //     //    attributesDictionary.Add(kvp.Key, kvp.Value);
        //     //}

        //     //return attributesDictionary;

        //     return JsonConvert.DeserializeObject<Dictionary<string, IAttribute>>(json);
        // }

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
