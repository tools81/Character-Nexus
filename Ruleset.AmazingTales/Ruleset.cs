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

        public string RulesetName => "Ruleset.AmazingTales";

        public string ImageSource => "https://drive.google.com/thumbnail?id=17mBZDgUuTiCbZgHcWmcWyiWS1bjZmor5&sz=w1024";

        public string LogoSource => "https://drive.google.com/thumbnail?id=1TdxuN20fe-1Dswj0uJ662wW_s_0VXpyd&sz=w256";

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
