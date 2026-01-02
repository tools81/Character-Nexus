using Newtonsoft.Json.Serialization;

namespace Utility
{
    public static class JsonContractResolver
    {
        public static DefaultContractResolver Get() => new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy
            {
                ProcessDictionaryKeys = false
            }
        };
    }
}