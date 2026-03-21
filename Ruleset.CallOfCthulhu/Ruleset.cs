using Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;

namespace CallOfCthulhu
{
    public class Ruleset : IRuleset
    {
        public string Name => "Call Of Cthulhu";
        public string RulesetName => "Ruleset.CallOfCthulhu";
        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_cthulhu.jpg";
        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_call_of_cthulhu.png";

        public string FormResource => "Ruleset.CallOfCthulhu.Json.Character.Form.json";
        public string  Instructions => File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Call_Of_Cthulhu_Instructions.html");
        public string Stylesheet => File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Call_Of_Cthulhu.css");

        public bool DeleteCharacter(string id)
        {
            throw new NotImplementedException();
        }

        public string LoadCharacter(ICharacter character)
        {
            return JsonConvert.SerializeObject(character, new CharacterJsonConverter());
        }

        public string NewCharacter()
        {
            string jsonObject;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(FormResource))
            {
                using (var reader = new StreamReader(stream))
                {
                    jsonObject = reader.ReadToEnd();
                }
            }

            return jsonObject;
        }

        public ICharacter SaveCharacter(string data)
        {
            try
            {
                var character = JsonConvert.DeserializeObject<Character>(data, new CharacterJsonConverter());
                return character;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing Json on save character: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving character: {ex.Message}");
                return null;
            }
        }
    }
}
