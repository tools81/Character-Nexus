using Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;

namespace EverydayHeroes
{
    public class Ruleset : IRuleset
    {
        public string Name => "Everyday Heroes";
        public string RulesetName => "Ruleset.EverydayHeroes";
        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_everyday_heroes.jpg";
        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_everyday_heroes.png";

        public string FormResource => "Ruleset.EverydayHeroes.Json.Character.Form.json";
        public string  Instructions => File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Everyday_Heroes_Instructions.html");
        public string Stylesheet => File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Everyday_Heroes.css");

        public Ruleset()
        {

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

        public ICharacter? SaveCharacter(string data)
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

        public string LoadCharacter(ICharacter character)
        {
            return JsonConvert.SerializeObject(character, new CharacterJsonConverter());
        }

        public bool DeleteCharacter(string id)
        {
            throw new NotImplementedException();
        }
    }
}
