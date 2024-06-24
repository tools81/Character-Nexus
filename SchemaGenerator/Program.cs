using EverydayHeroes;


namespace SchemaGenerator
{
    public class Program
    {
        private static List<object> _fields = new List<object>();

        static void Main(string[] args)
        {
            GenerateFormSchema.InitializeSchema();
        }        
    }
}
