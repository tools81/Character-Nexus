using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Utility;

namespace Template
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = $"{new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.Parent}/Ruleset.Template/Json/";
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = JsonContractResolver.Get(),
            Formatting = Formatting.None
        };

        public static void InitializeSchema()
        {
        }
    }
}
