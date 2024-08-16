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
        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_amazing_tales.jpg";
        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_amazing_tales.png";

        public Ruleset()
        {
            
        }

        public string NewCharacter()
        {
            string jsonObject;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.AmazingTales.Json.Character.Form.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    jsonObject = reader.ReadToEnd();
                }
            }

            return jsonObject;
        }

        public ICharacter? SaveCharacter(string data)
        {
            throw new NotImplementedException();
        }

        public string LoadCharacter(ICharacter character)
        {
            throw new NotImplementedException();
        }
    }
}
