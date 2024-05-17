using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace AmazingTales
{
    public class Ruleset : IRuleset
    {
        public string Name => "Amazing Tales";

        public ICharacter NewCharacter()
        {
            var character = new Character();

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AmazingTales.Json.Attributes.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var jsonContent = reader.ReadToEnd();
                    character.Attributes = JsonTo.Dictionary<Attribute>(jsonContent);
                }
            }

            return character;
        }
    }
}
