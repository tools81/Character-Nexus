using System.Reflection;
using System.Text.RegularExpressions;

namespace Template
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = Assembly.GetExecutingAssembly().Location + "/Json/";
        private static readonly Regex sWhitespace = new Regex(@"\s+");

        public static void InitializeSchema()
        {
        }
    }
}
