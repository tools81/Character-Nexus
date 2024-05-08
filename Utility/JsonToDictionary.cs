using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Interfaces;

namespace Utility
{
    public static class JsonToDictionary
    {
        public static Dictionary<string, IAttribute> ConvertAttribute<IAttribute>(string json)
        {
            //// Deserialize the JSON into a dictionary
            //var attributesData = JsonConvert.DeserializeObject<Dictionary<string, IAttribute>>(json);

            //// Create a dictionary to hold the parsed data
            //var attributesDictionary = new Dictionary<string, IAttribute>();

            //foreach (var kvp in attributesData["Attributes"])
            //{
            //    attributesDictionary.Add(kvp.Key, kvp.Value);
            //}

            //return attributesDictionary;

            return JsonConvert.DeserializeObject<Dictionary<string, IAttribute>>(json);
        }

        public static Dictionary<string, IAbility> ConvertAbility<IAbility>(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, IAbility>>(json);
        }
    }
}
