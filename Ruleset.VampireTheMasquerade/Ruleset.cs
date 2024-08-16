using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace VampireTheMasquerade
{
    public class Ruleset : IRuleset
    {
        public string Name => "Vampire: The Masquerade";
        public string RulesetName => "Ruleset.VampireTheMasquerade";
        public string ImageSource => "";
        public string LogoSource => "";

        public Ruleset()
        {
            
        }

        public string NewCharacter()
        {
            string jsonObject;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.VampireTheMasquerade.Json.Character.Form.json"))
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
