using EverydayHeroes;
using Marvel;


namespace SchemaGenerator
{
    public class Program
    {
        private static List<object> _fields = new List<object>();

        static void Main(string[] args)
        {
            EverydayHeroes.GenerateFormSchema.InitializeSchema();
            Marvel.GenerateFormSchema.InitializeSchema();

            Console.Read();
        }        
    }
}
