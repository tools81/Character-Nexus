﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Ghostbusters
{
    public class Ruleset : IRuleset
    {
        public string Name => "Ghostbusters";
        public string RulesetName => "Ruleset.Ghostbusters";
        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_ghostbusters.jpg";
        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_ghostbusters.png";
        public string FormResource => "Ruleset.Ghostbusters.Json.Character.Form.json";

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
