using Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace EverydayHeroes
{
    public class Ruleset : IRuleset
    {
        public string Name => "Everyday Heroes";
        public string RulesetName => "Ruleset.EverydayHeroes";
        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_everyday_heroes.jpg";
        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_everyday_heroes.png";

        public string FormResource => "Ruleset.EverydayHeroes.Json.Character.Form.json";

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

        public IEnumerable<Attribute> GetAttributeList()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Attributes.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var jsonContent = reader.ReadToEnd();
                    return JsonTo.IEnumerable<Attribute>(jsonContent);
                }
            }
        }

        public IEnumerable<IClass> GetClassList()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Classes.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var jsonContent = reader.ReadToEnd();
                    return JsonTo.IEnumerable<Archetype>(jsonContent);
                }
            }
        }

        public IEnumerable<IFeature> GetFeatureList()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Features.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var jsonContent = reader.ReadToEnd();
                    return JsonTo.IEnumerable<Feature>(jsonContent);
                }
            }
        }

        public IEnumerable<IAbility> GetAbilityList()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Abilities.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var jsonContent = reader.ReadToEnd();
                    return JsonTo.IEnumerable<Ability>(jsonContent);
                }
            }
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
