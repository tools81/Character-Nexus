using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Utility
{
    public static class LoadJson
    {
        public static List<T> Load<T>(string jsonFilesPath, string fileName)
        {
            string json = File.ReadAllText(jsonFilesPath + fileName);
            var result = JsonConvert.DeserializeObject<List<T>>(json);
            if (result == null) { Console.WriteLine($"Unable to read {fileName}. Aborting..."); Console.Read(); return new List<T>(); }
            return result;
        }
    }
}
