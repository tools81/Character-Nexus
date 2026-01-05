namespace SchemaGenerator
{
    public class Program
    {
        private static List<object> _fields = new List<object>();

        static void Main(string[] args)
        {
            AmazingTales.GenerateFormSchema.InitializeSchema();
            BladeRunner.GenerateFormSchema.InitializeSchema();
            DarkCrystal.GenerateFormSchema.InitializeSchema();
            //EverydayHeroes.GenerateFormSchema.InitializeSchema();
            Ghostbusters.GenerateFormSchema.InitializeSchema();
            Marvel.GenerateFormSchema.InitializeSchema();

            Console.Read();
        }        
    }
}
