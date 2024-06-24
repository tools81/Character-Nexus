using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace EverydayHeroes
{
    public class Ruleset : IRuleset
    {
        public string Name => "Everyday Heroes";

        public string RulesetName => "Ruleset.EverydayHeroes";

        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_everyday_heroes.jpg";

        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_everyday_heroes.png";

        public Ruleset()
        {
            
        }

        // public ICharacter NewCharacter()
        // {
        //     var character = new Character();

        //     using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EverydayHeroes.Json.Attributes.json"))
        //     {
        //         using (var reader = new StreamReader(stream))
        //         {
        //             var jsonContent = reader.ReadToEnd();
        //             character.Attributes = JsonTo.List<Attribute>(jsonContent);
        //         }
        //     }

        //     using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EverydayHeroes.Json.Skills.json"))
        //     {
        //         using (var reader = new StreamReader(stream))
        //         {
        //             var jsonContent = reader.ReadToEnd();
        //             character.Skills = JsonTo.List<Skill>(jsonContent);
        //         }
        //     }

        //     return character;
        // }

        public string NewCharacter()
        {
            string jsonObject;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ruleset.EverydayHeroes.Json.Schema.Form.json"))
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
    }
}
